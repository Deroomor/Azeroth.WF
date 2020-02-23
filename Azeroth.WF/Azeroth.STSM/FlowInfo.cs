using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    public enum HandlerOptions
    {
        提交=0,
        同意=1,
        不同意=2,
        撤回=3,
        删除=4
    }

    public enum HandlerTypes
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
        处理中=0,
        已完成=1,
        被驳回=2,
        已删除=3,
        已通过=4
            
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
        /// 处理人Id
        /// </summary>
        public long HandlerId { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandlerTime { get; set; }

        /// <summary>
        /// 处理人姓名 冗余
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings =true)]
        public string HandlerName { get; set; }

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
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string FlowContext { get; set; }

        /// <summary>
        /// 处理场景
        /// </summary>
        public HandlerTypes HandlerType { get; set; }

        /// <summary>
        /// WF的Id 外键
        /// </summary>
        public Guid WFInstanceId { get; set; }

        /// <summary>
        /// 记录状态（逻辑）
        /// </summary>
        public RowStates RowState { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        public FlowStates FlowState { get; set; }

    }
}
