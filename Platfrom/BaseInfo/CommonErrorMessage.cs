
namespace Gsafety.PTMS.BaseInfo
{
    public class CommonErrorMessage
    {
        /// <summary>
        /// 保存数据库失败
        /// </summary>
        public const string SaveDBFailed = "SaveDBFailed!";
        /// <summary>
        /// 参数无效
        /// </summary>
        public const string ParameterInvalid = "Parameterinvalid!";
        //public const string IDCannotBeNull = "ID can not be null!";

        #region OrderClient
        /// <summary>
        /// 客户用户不存在
        /// </summary>
        public const string OrderClientNotExist = "orderclientnotexist";
        public const string OrderClientRoleNameNoExist = "OrderClientRoleNameNoExist";
        #endregion

        #region Guser
        /// <summary>
        /// 账号已存在
        /// </summary>
        public const string AccountExist = "Accountexists";

        /// <summary>
        /// 账号不存在
        /// </summary>
        public const string AccountNoExist = "Accountnoexists";

        /// <summary>
        /// 客户端不存在管理员
        /// </summary>
        public const string OrderClientAdminNoExist = "OrderClientAdminnoexists";
        #endregion

        #region Role

        /// <summary>
        /// 角色不存在
        /// </summary>
        public const string RoleNotExist = "rolenotexist";

        /// <summary>
        /// 此角色已包含用户
        /// </summary>
        public const string RoleContainUser = "Therolecontainsusers";
        #endregion

        #region UsrDepatment

        /// <summary>
        /// 用户部门不存在
        /// </summary>
        public const string UerDepartmentNoExist = "UerDepartmentnoexists";

        #endregion
    }
}
