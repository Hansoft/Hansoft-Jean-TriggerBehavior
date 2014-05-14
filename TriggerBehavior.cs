using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

using HPMSdk;
using Hansoft.ObjectWrapper;

using Hansoft.Jean.Behavior;
using System.Data;
using System.Diagnostics;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics;


namespace Hansoft.Jean.Behavior.TriggerBehavior
{
    /// <summary>
    /// TriggerBehavior is a behavior part of Jean that allows actions to be triggered when tasks changes and a condition on the specific task is matched.
    /// Typically this can be used when you want to set column values based of some other column, or if you want to make an arithmetic calculation on one
    /// column, where the value should be put into another column.

    /// Syntax:
    /// ---------------------------------------------------------------------------------
    /// Column syntax: $COLUMNNAME (case sensitive)
    /// String syntax: 'String'
    /// Constants: TIMENOW
    /// Operators: +,-,/,*,<,<=,>,>=,!=,==
    /// Paranthesis to encapsulate expressions
    /// List syntax: [item1,item2,item3]
    /// 
    /// 
    /// Example syntax:
    /// <TriggerBehavior HansoftProject="Test Cases" View="Backlog">
    ///       <Condition Expression="$WorkflowStatus == 'Test Passed'">
    ///         $Last Run = TIMENOW
    ///         $Latest Pass = TIMENOW
    ///         $Times Run = $Times Run + 1
    ///         $Times Passed = $Times Passed + 1
    ///       </Condition>
    ///       <Condition Expression="$WorkflowStatus == 'Test Failed'">
    ///         $Last Run = TIMENOW
    ///         $Times Run = $Times Run + 1
    ///       </Condition>
    ///       <Condition Expression="$Times Run > 5">
    ///         $Test steps = $Test steps + 'Number of times run: '+ $Times Run +'\n'
    ///       </Condition>
    ///     </TriggerBehavior>    
    ///     
    /// Exceptions in the parsing/execution process will be Logged in the Windows Event Viewer.
    /// </summary>
    public class TriggerBehavior : AbstractBehavior
    {
        // The projects being monitored
        string projectQuery;
        // The view being monitored
        EHPMReportViewType viewType;

        // The list of projects we're monitoring
        List<Project> projects;
        // Is the project query an exclusive query
        bool inverted = false;
        // The lsit of project views being monitored
        List<ProjectView> projectViews;
        // The lsit of triggers in the behavior
        List<Condition> conditions;
        string title;
        bool initializationOK = false;

        public TriggerBehavior(XmlElement configuration)
            : base(configuration) 
        {
            projectQuery = GetParameter("HansoftProject");
            string invert = GetParameter("InvertedMatch");
            if (invert != null)
                inverted = invert.ToLower().Equals("yes");
            viewType = GetViewType(GetParameter("View"));
            conditions = GetConditions(configuration);
            title = "TriggerBehavior: " + configuration.InnerText;
        }

        public override string Title
        {
            get { return title; }
        }
        
        /// <summary>
        /// Initializes all projects after the session has been connected.
        /// </summary>
        private void InitializeProjects()
        {
            projects = new List<Project>();
            projectViews = new List<ProjectView>();
            projects = HPMUtilities.FindProjects(projectQuery, inverted);
            foreach (Project project in projects)
            {
                ProjectView projectView;
                if (viewType == EHPMReportViewType.AgileBacklog)
                    projectView = project.ProductBacklog;
                else if (viewType == EHPMReportViewType.AllBugsInProject)
                    projectView = project.BugTracker;
                else
                    projectView = project.Schedule;

                projectViews.Add(projectView);
            }
        }

        /// <summary>
        /// Called when the session is connected
        /// </summary>
        public override void Initialize()
        {
            initializationOK = false;
            InitializeProjects();
            initializationOK = true;
        }


        /// <summary>
        /// Extracts all conditions and their associated statements from the XML. 
        /// XML Syntax:
        ///  <Condition Expression="<conditional expression>"> 
        ///     assignement statement (must contain a =) 
        ///     assignement statement (must contain a =) 
        ///     assignement statement (must contain a =) 
        ///     assignement statement (must contain a =) 
        /// </Condition>
        ///  <Condition Expression="<conditional expression>"> 
        ///     assignement statement (must contain a =) 
        ///     assignement statement (must contain a =) 
        /// </Condition>
        /// 
        /// TODO: Fix the tokenizer so that = is an operator as any other and allow
        /// for custom actions to happens (such as for example send mail)
        /// </summary>
        /// <param name="parent">the xml element containing all the XML elements.</param>
        /// <returns></returns>
        private List<Condition> GetConditions(XmlElement parent)
        {
            List<Condition> conditions = new List<Condition>();
            List<string> errors = new List<string>();
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement el = (XmlElement)node;
                    Condition condition = new Condition();
                    condition.parse(el.GetAttribute("Expression"), ref errors);
                    string[] expressions = el.InnerText.Split('\n');
                    foreach (string expression in expressions)
                    {
                        //Only make assignments from the expressions that contains an assignment operator
                        if(expression.Contains("="))
                        {
                            Assignment assignment = new Assignment();
                            assignment.parse(expression, ref errors);
                            condition.AddAssigment(assignment);
                        }
                    }
                    condition.FindInfiniteLoops(ref errors);
                    conditions.Add(condition);
                }
            }
            if (errors.Count > 0)
                throw new ArgumentException("Found errors in Triggerbehavior: " + errors.ToString());
            return conditions;
        }

        /// <summary>
        /// Returns true if the task is in the subset of project views this behaviour is monitoring
        /// </summary>
        /// <param name="task">The task to check if it's in any of the views</param>
        /// <returns>true if the task is in the monitored scope</returns>
        private Boolean IsTaskInViews(Task task)
        {
            foreach (ProjectView projectView in projectViews)
            {
                if (task.ProjectView.UniqueID == projectView.UniqueID)
                    return true;
            }
            return false;
        }

        // TODO: Subject to refactoring
        private EHPMReportViewType GetViewType(string viewType)
        {
            switch (viewType)
            {
                case ("Agile"):
                    return EHPMReportViewType.AgileMainProject;
                case ("Scheduled"):
                    return EHPMReportViewType.ScheduleMainProject;
                case ("Bugs"):
                    return EHPMReportViewType.AllBugsInProject;
                case ("Backlog"):
                    return EHPMReportViewType.AgileBacklog;
                default:
                    throw new ArgumentException("Unsupported View Type: " + viewType);
            }
        }

        /// <summary>
        /// Checks if the change on the task triggers any conditions. If so all the assignments in that
        /// condition will be executed.
        /// </summary>
        /// <param name="task">the task that has changed</param>
        /// <param name="fieldChanged">the field that has changed</param>
        /// <param name="columnHash">if it was a custom column the hash for the column must be included</param>
        private void EvaluateTask(Task task, EHPMTaskField fieldChanged, uint columnHash = 0)
        {
            // Check if the task is in scope
            if(IsTaskInViews(task))
            {
                string columnName = "";
                if (columnHash > 0)
                {
                    HPMProjectCustomColumnsColumn column = task.ProjectView.GetCustomColumn(columnHash);
                    if (column != null)
                        columnName = column.m_Name;
                }
                ListenerData trigger = new ListenerData(fieldChanged, columnName);
                foreach (Condition condition in conditions)
                {
                    // Check if the condition actually cares about this change (for performance) before evaluating.
                    if (condition.AffectedBy().Contains(trigger) && condition.Evaluate(task))
                    {
                        Debug.WriteLine("Trigger: " + fieldChanged + "-" + columnName);
                        Debug.WriteLine("Condition (" + condition.ConditionalExpression + ") activated for " + task.Name);
                        condition.ExcuteAssignments(task);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a task has changed.
        /// </summary>
        /// <param name="e">the data containing information about the change</param>
        public override void OnTaskChange(TaskChangeEventArgs e)
        {
            if (initializationOK)
            {
                Task task = Task.GetTask(e.Data.m_TaskID);
                EvaluateTask(task,  e.Data.m_FieldChanged);
            }
        }

        /// <summary>
        /// Called when a custom column is changed on a task.
        /// </summary>
        /// <param name="e">the data containing information about the change</param>
        public override void OnTaskChangeCustomColumnData(TaskChangeCustomColumnDataEventArgs e)
        {
            if (initializationOK)
            {
                Task task = Task.GetTask(e.Data.m_TaskID);
                EvaluateTask(task, EHPMTaskField.CustomColumnData, e.Data.m_ColumnHash);
            }
        }

        /// <summary>
        /// We are not buffering events in this behavior
        /// </summary>
        /// <param name="e"></param>
        public override void OnBeginProcessBufferedEvents(EventArgs e)
        {
        }

        /// <summary>
        /// We are not buffering events in this behavior
        /// </summary>
        /// <param name="e"></param>
        public override void OnEndProcessBufferedEvents(EventArgs e)
        {
        }

        /// <summary>
        /// TODO: Possibly add triggers when tasks are created.
        /// </summary>
        /// <param name="e"></param>
        public override void OnTaskCreate(TaskCreateEventArgs e)
        {
        }

        /// <summary>
        /// TODO: Possibly add triggers when tasks are moved.
        /// </summary>
        /// <param name="e"></param>
        public override void OnTaskMove(TaskMoveEventArgs e)
        {
        }
    }
}
