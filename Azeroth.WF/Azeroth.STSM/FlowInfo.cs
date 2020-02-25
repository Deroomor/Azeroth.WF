using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    /// <summary>
    /// 操作类别
    /// </summary>
    public enum HandlerOptions
    {
        新建=0,
        同意=1,
        驳回=2,
        撤回=3,
        删除=4
    }

    /// <summary>
    /// 场景类别
    /// </summary>
    public enum NodeTypes
    {
        单人审批=0,
        多人审批=1
    }

    public enum RowStates
    {
        正常=0,
        归档=1
    }

    /// <summary>
    /// 流程状态
    /// </summary>
    public enum FlowStates
    {
        未提交=0,
        处理中=1,
        已完结=2,
    }

    /// <summary>
    /// 流程结果
    /// </summary>
    public enum FlowResults
    {
        None=0,
        通过=1,
        驳回=2
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
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 当前处理人Id
        /// </summary>
        public long HandlerId { get; set; }

        /// <summary>
        /// 当前处理人处理时间
        /// </summary>
        public DateTime HandlerTime { get; set; }

        /// <summary>
        /// 当前处理人姓名 冗余
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings =true)]
        public string HandlerName { get; set; }

        /// <summary>
        /// 上一步处理人Id
        /// </summary>
        public long PreviousHandlerId { get; set; }

        /// <summary>
        /// 上一步处理人处理时间
        /// </summary>
        public DateTime PreviousHandlerTime { get; set; }

        /// <summary>
        /// 上一步处理人姓名 冗余
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string PreviousHandlerName { get; set; }

        /// <summary>
        /// 操作类别 
        /// </summary>
        public HandlerOptions HandlerOption { get; set; }

        /// <summary>
        /// 流程节点编号（用于画流程图）
        /// </summary>
        public Guid FlowNo { get; set; }

        /// <summary>
        /// 流程节点编号（父级）（用于画流程图）
        /// </summary>
        public Guid FlowNoParent { get; set; }

        /// <summary>
        /// 流程分组名称（用于画流程图）
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string FlowGroupName { get; set; }

        /// <summary>
        /// 流程上下文，表单数据
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string FlowContext { get; set; }

        /// <summary>
        /// 场景类别
        /// </summary>
        public NodeTypes NodeType { get; set; }

        /// <summary>
        /// WF的Id 外键
        /// </summary>
        public Guid WFInstanceId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public RowStates RowState { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        public FlowStates FlowState { get; set; }

        /// <summary>
        /// 流程结果
        /// </summary>
        public FlowResults FlowResult { get; set; }
        /// <summary>
        /// 书签，配合WF的持久化
        /// </summary>
        public string BookmarkName { get; set; }

    }
}
