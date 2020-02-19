using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    public enum OperationTypes
    {
        提交=0,
        同意=1,
        不同意=2,
        撤回=3,
        删除=4
    }

    public enum ApproveTypes
    {
        单人审批=0,
        多人审批=1
    }

    public enum RowStates
    {
        正常=0,
        归档=1
    }

    public enum FlowStates
    {
        审批中=0,
        通过=1,
        未通过=2,
        已删除=3
            
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("FlowInfo")]
    public class FlowInfo
    {
        /// <summary>
        /// 逻辑Id
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public long CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人姓名，冗余
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 审批人Id
        /// </summary>
        public long ApproverId { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApproveTime { get; set; }

        /// <summary>
        /// 审批人姓名 冗余
        /// </summary>
        public string ApproverName { get; set; }

        /// <summary>
        /// 操作类别 
        /// </summary>
        public OperationTypes OperationType { get; set; }

        /// <summary>
        /// 操作标识（用于画流程图）
        /// </summary>
        public Guid HandleNo { get; set; }

        /// <summary>
        /// 上级操作标志（用于画流程图）
        /// </summary>
        public Guid HandleNoParent { get; set; }

        /// <summary>
        /// 操作分组名称（用于画流程图）
        /// </summary>
        public string HandleGroupName { get; set; }

        /// <summary>
        /// 流程上下文，表单数据
        /// </summary>
        public string FlowContext { get; set; }

        /// <summary>
        /// 审批类别
        /// </summary>
        public ApproveTypes ApproveType { get; set; }

        /// <summary>
        /// WF的Id 外键
        /// </summary>
        public Guid WFInstanceId { get; set; }

        /// <summary>
        /// 数据状态-逻辑
        /// </summary>
        public RowStates RowState { get; set; }

        public FlowStates FlowState { get; set; }

    }
}
