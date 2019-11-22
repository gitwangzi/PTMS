using Jounce.Core.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Gsafety.PTMS.Bases.Librarys
{
    /// <summary>
    ///树形结构类
    /// </summary>
    public class TreeNode : BaseNotify
    {
        public TreeNode()
        {
            Children = new ObservableCollection<TreeNode>();
        }

        /// <summary>
        /// 是否是叶子节点，即是否是车辆
        /// </summary>
        public bool IsLeaf { get; protected set; }

        public TreeNode Parent { get; set; }

        public bool HasChildren
        {
            get
            {
                if (Children == null)
                {
                    return false;
                }
                else
                {
                    return Children.Count > 0;
                }
            }
        }

        private Visibility _visibility;
        /// <summary>
        /// 用于数的搜索
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    RaisePropertyChanged(() => Visibility);
                }
            }
        }

        public ObservableCollection<TreeNode> Children
        {
            get;
            set;
        }
    }

    public class TreeNode<TModel> : BaseNotify, ITreeNode<TModel>
        where TModel : class
    {
        public TreeNode()
        {
            Children = new ObservableCollection<ITreeNode>();
        }

        public TModel Model { get; set; }

        public ITreeNode Parent { get; set; }

        public bool HasChildren
        {
            get
            {
                if (Children == null)
                {
                    return false;
                }
                else
                {
                    return Children.Count > 0;
                }
            }
        }

        private Visibility _visibility;
        /// <summary>
        /// 用于数的搜索
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    RaisePropertyChanged(() => Visibility);
                }
            }
        }

        public ObservableCollection<ITreeNode> Children
        {
            get;
            set;
        }
    }


    public interface ITreeNode<out TModel> : ITreeNode
    {

    }

    public interface ITreeNode
    {

    }
}
