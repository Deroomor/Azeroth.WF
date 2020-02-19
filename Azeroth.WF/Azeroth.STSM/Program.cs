using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("0----发起请假");
            Console.WriteLine("1----部门领导登陆");
            Console.WriteLine("2----Hr登陆");
            Console.WriteLine("Q-----退出");
            ConsoleKey input;
            while ((input=Console.ReadKey().Key)!=ConsoleKey.Q)
            {
                if (input == ConsoleKey.D0)
                    Start();
                else if (input == ConsoleKey.D1)
                    MasterHandler();
                else if (input == ConsoleKey.D2)
                    HrHandler();
            }
        }

        private static void HrHandler()
        {
            throw new NotImplementedException();
        }

        private static void MasterHandler()
        {
            throw new NotImplementedException();
        }

        private static void Start()
        {
            DbContext dbcontext = new DbContext();
            FlowInfo fl = new FlowInfo()
            {
                ApproveType = ApproveTypes.单人审批,
                ApproveTime = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatorId = 0,
                CreatorName = "员工",
                FlowState = FlowStates.审批中,
                HandleGroupName = "提交",
                HandleNo = Guid.NewGuid(),
                HandleNoParent = Guid.Empty,
                Id = DateTime.Now.Ticks,
                OperationType = OperationTypes.提交,
                RowState = RowStates.正常,
                ApproverId = 0,
                ApproverName = "员工"
            };

            LeaveInput input = new LeaveInput();
            Console.WriteLine("请输入请假的小时数");
            input.Hours = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入请假理由");
            input.Remark = Console.ReadLine();
            fl.FlowContext = Newtonsoft.Json.JsonConvert.SerializeObject(input);

            

            System.Activities.WorkflowApplication wfa = new System.Activities.WorkflowApplication(new LeaveFlow());

            string cnnstr = System.Configuration.ConfigurationManager.ConnectionStrings["master"].ConnectionString;
            var sqlstore= new System.Activities.DurableInstancing.SqlWorkflowInstanceStore(cnnstr);
            var handler= sqlstore.CreateInstanceHandle();
            var view= sqlstore.Execute(handler,new System.Activities.DurableInstancing.CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));
            sqlstore.DefaultInstanceOwner = view.InstanceOwner;
            handler.Free();
            wfa.InstanceStore = sqlstore;

            fl.WFInstanceId = wfa.Id;
            dbcontext.FlowInfo.Add(fl);
            dbcontext.SaveChanges();

            wfa.Run();

        }

        public class LeaveInput
        {
            public int Hours { get; set; }

            public string Remark { get; set; }
        }
    }
}
