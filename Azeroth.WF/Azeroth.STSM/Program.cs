using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContext db = new DbContext();
            var lst= db.FlowInfo.ToList();

            Console.WriteLine("Y----员工登陆");
            Console.WriteLine("S----上级登陆");
            Console.WriteLine("H----HR登陆");
            Console.WriteLine("Q-----退出");
            ConsoleKey input;
            while ((input=Console.ReadKey().Key)!=ConsoleKey.Q)
            {
                try
                {
                    if (input == ConsoleKey.Y)
                        Start((int)Role.员工, "张三");
                    else if (input == ConsoleKey.S)
                        ProcessHandler((int)Role.上级);
                    else if (input == ConsoleKey.H)
                        ProcessHandler((int)Role.HR);
                    else
                        throw new ArgumentException("未识别的操作");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                }
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Y----员工登陆");
                Console.WriteLine("S----上级登陆");
                Console.WriteLine("H----HR登陆");
                Console.WriteLine("Q-----退出");
            }
        }


        private static void ProcessHandler(int userId)
        {
            //获取待处理的流程
            DbContext dbcontext = new DbContext();
            var lst= dbcontext.FlowInfo.Where(x => x.RowState == RowStates.正常)
                .Where(x => x.HandlerId == userId)
                .ToList();
            if(lst.Count<=0)
            {
                Console.WriteLine("没有待处理的流程");
                return;
            }
            int indexTMP = 0;
            lst.ForEach(x =>Console.WriteLine(string.Format("索引：{0}，流程ID：{1}，处理人ID：{2}，处理人：{3}，表单：{4}", 
                indexTMP++, x.WFInstanceId, x.HandlerId, x.HandlerName,x.FlowContext)));
            Console.WriteLine("请输入流程索引");
            int index = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入审批意见");
            Console.WriteLine(string.Format("{0}---{1}", (int)HandlerOptions.同意, HandlerOptions.同意.ToString()));
            Console.WriteLine(string.Format("{0}---{1}", (int)HandlerOptions.驳回, HandlerOptions.驳回.ToString()));
            HandlerOptions opt = (HandlerOptions)Convert.ToInt32(Console.ReadLine());
            //组织表单数据
            var formData= Newtonsoft.Json.Linq.JObject.Parse(lst[index].FlowContext);
            formData["HandlerOption"] = Newtonsoft.Json.Linq.JToken.FromObject(opt);//demo,这个键写死
            //启动流程
            System.Activities.WorkflowApplication wfa = new System.Activities.WorkflowApplication(new LeaveFlow());
            InitWF(wfa);
            wfa.Load(lst[index].WFInstanceId);
            wfa.ResumeBookmark(lst[index].BookmarkName, formData);
        }

        private static void Start(int userId,string userName)
        {
            DbContext dbcontext = new DbContext();
            FlowInfo fl = new FlowInfo()
            {
                Id = DateTime.Now.Ticks,
                CreatedTime = DateTime.Now,
                CreatorId = userId,
                CreatorName = userName,
                FlowGroupName = "开始",
                FlowNo = Guid.NewGuid(),
                FlowNoParent = Guid.Empty,
                FlowResult = FlowResults.None,
                FlowState = FlowStates.未提交,
                HandlerId = userId,
                HandlerName = userName,
                HandlerOption = HandlerOptions.新建,
                NodeType = NodeTypes.单人审批,
                HandlerTime = DateTime.Now,
                PreviousHandlerId = -1,
                PreviousHandlerName = string.Empty,
                PreviousHandlerTime = DateTime.Now,
                RowState = RowStates.正常,
                 WFInstanceId=Guid.Empty,
            };

            LeaveInput input = new LeaveInput();
            Console.WriteLine("请输入请假的小时数");
            input.Hours = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入请假理由");
            input.Remark = Console.ReadLine();
            fl.FlowContext = Newtonsoft.Json.JsonConvert.SerializeObject(input);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("flowInfo", fl);
            System.Activities.WorkflowApplication wfa = new System.Activities.WorkflowApplication(new LeaveFlow(),dict);
            fl.WFInstanceId = wfa.Id;
            InitWF(wfa);
            dbcontext.FlowInfo.Add(fl);
            dbcontext.SaveChanges();
            wfa.Run();
        }

        private static void InitWF(WorkflowApplication wfa)
        {
            string cnnstr = System.Configuration.ConfigurationManager.ConnectionStrings["master"].ConnectionString;
            var sqlstore = new System.Activities.DurableInstancing.SqlWorkflowInstanceStore(cnnstr);
            var handler = sqlstore.CreateInstanceHandle();
            var view = sqlstore.Execute(handler, new System.Activities.DurableInstancing.CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));
            sqlstore.DefaultInstanceOwner = view.InstanceOwner;
            handler.Free();
            wfa.InstanceStore = sqlstore;

            wfa.Aborted = AbortedHandler;
            wfa.OnUnhandledException = OnUnhandledExceptionHandler;
            wfa.Completed = CompletedHandler;
            wfa.PersistableIdle = PersistableIdleHandler;
        }

        private static PersistableIdleAction PersistableIdleHandler(WorkflowApplicationIdleEventArgs arg)
        {
            Console.WriteLine("PersistableIdleHandler");
            return PersistableIdleAction.Unload;
        }

        private static void CompletedHandler(WorkflowApplicationCompletedEventArgs obj)
        {
            Console.WriteLine("CompletedHandler:{0}", obj.CompletionState.ToString());
            if (obj.CompletionState== ActivityInstanceState.Closed)
            {
                DbContext dbcontext = new DbContext();
                var flowInfo= dbcontext.FlowInfo.Where(x => x.WFInstanceId == obj.InstanceId)
                    .Where(x => x.RowState == RowStates.正常)
                    .Where(x => x.FlowState == FlowStates.处理中)
                    .Where(x => x.FlowResult == FlowResults.None)
                    .FirstOrDefault();
                flowInfo.FlowResult = FlowResults.通过;
                dbcontext.SaveChanges();
            }
            //其他情况后续完善
            
        }

        private static UnhandledExceptionAction OnUnhandledExceptionHandler(WorkflowApplicationUnhandledExceptionEventArgs arg)
        {
            Console.WriteLine("OnUnhandledExceptionHandler");
            return UnhandledExceptionAction.Terminate;
        }

        private static void AbortedHandler(WorkflowApplicationAbortedEventArgs obj)
        {
            Console.WriteLine("AbortedHandler");
           
        }
    }

    /// <summary>
    /// 请假的表单
    /// </summary>
    public class LeaveInput
    {
        public int Hours { get; set; }

        public string Remark { get; set; }

        public HandlerOptions HandlerOption { get; set; }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum Role
    {
        员工 = 0,
        上级 = 1,
        HR = 2
    }

    
}
