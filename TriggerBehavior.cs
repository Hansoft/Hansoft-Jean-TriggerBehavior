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
    public class TriggerBehavior : AbstractBehavior
    {
        string projectName;
        EHPMReportViewType viewType;
        List<Project> projects;
        bool inverted = false;
        List<ProjectView> projectViews;
        List<Condition> conditions;
        string title;
        bool initializationOK = false;

        public TriggerBehavior(XmlElement configuration)
            : base(configuration) 
        {
            projectName = GetParameter("HansoftProject");
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
        
        private void InitializeProjects()
        {
            projects = new List<Project>();
            projectViews = new List<ProjectView>();
            projects = HPMUtilities.FindProjects(projectName, inverted);
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

        public override void Initialize()
        {
            initializationOK = false;
            InitializeProjects();
            initializationOK = true;
        }


        // TODO: Subject to refactoring
        private List<Condition> GetConditions(XmlElement parent)
        {
            List<Condition> condtitions = new List<Condition>();
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement el = (XmlElement)node;
                    Condition condition = new Condition(el.GetAttribute("Expression"));
                    foreach (XmlNode assignmentNode in el.ChildNodes)
                    {
                        if (assignmentNode is XmlElement)
                        {
                            XmlElement assignmentElem = (XmlElement)assignmentNode;
                            Assignment assignment = Assignment.parse(assignmentElem.GetAttribute("Expression"));
                            condition.AddAssigment(assignment);
                        }
                    }
                    condtitions.Add(condition);
                }
            }
            return condtitions;
        }

        //Returns true if the task is in the subset of project views this behaviour is monitoring
        private Boolean isTaskInViews(Task task)
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

        public override void OnBeginProcessBufferedEvents(EventArgs e)
        {
        }

        public override void OnEndProcessBufferedEvents(EventArgs e)
        {            
        }

        private void EvaluateTask(Task task, EHPMTaskField fieldChanged, uint columnHash)
        {
            if(isTaskInViews(task))
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
                    if (condition.AffectedBy().Contains(trigger) && condition.Evaluate(task))
                    {
                        Debug.WriteLine("Trigger: " + fieldChanged + "-" + columnName);
                        Debug.WriteLine("Condition (" + condition.ExpressionStr + ") activated for " + task.Name);
                        condition.ExcuteAssignments(task);
                    }
                }
            }
        }
        public override void OnTaskChange(TaskChangeEventArgs e)
        {
            if (initializationOK)
            {
                Task task = Task.GetTask(e.Data.m_TaskID);
                EvaluateTask(task,  e.Data.m_FieldChanged, 0);
            }
        }

        public override void OnTaskChangeCustomColumnData(TaskChangeCustomColumnDataEventArgs e)
        {
            if (initializationOK)
            {
                Task task = Task.GetTask(e.Data.m_TaskID);
                EvaluateTask(task, EHPMTaskField.CustomColumnData, e.Data.m_ColumnHash);
            }
        }

        public override void OnTaskCreate(TaskCreateEventArgs e)
        {
        }

        public override void OnTaskDelete(TaskDeleteEventArgs e)
        {
        }

        public override void OnTaskMove(TaskMoveEventArgs e)
        {
            /* TODO: Figure out which tasks was moved.
             if (initializationOK)
            {
                foreach (HPMChangeCallbackData_TaskCreateUnifiedTask taskData in e.Data.)
                {
                    Task task = Task.GetTask(taskData.m_TaskRefID);
                    EvaluateTask(task);
                }
            }*/
        }
    }
}
