using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Gsafety.PTMS.Bases.Librarys
{
    public class TreeNodeFactory
    {
        #region 非泛型
        /// <summary>
        /// 创建一棵树，只有一个根节点
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <param name="idSelector"></param>
        /// <param name="parentIDSelector"></param>
        /// <returns></returns>
        public static TModel CreateTreeFromModelList<TModel>(List<TModel> models, Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Func<TModel, bool> rootFinder)
             where TModel : TreeNode, new()
        {
            var rootModel = models.FirstOrDefault(rootFinder);

            var dic = models.Where(t => null != parentIDSelector(t)).GroupBy(parentIDSelector).ToDictionary(a => a.Key, b => b.ToList());
            var tree = CreateTree(idSelector, parentIDSelector, dic, rootModel);

            return tree;
        }

        /// <summary>
        /// 创建森林，多棵树，多个根节点
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <param name="idSelector"></param>
        /// <param name="parentIDSelector"></param>
        /// <returns></returns>
        public static ObservableCollection<TModel> CreateTreeForestFromModelList<TModel>(List<TModel> models, Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Func<TModel, bool> rootFinder)
             where TModel : TreeNode, new()
        {
            var forest = new ObservableCollection<TModel>();
            var rootModels = models.Where(rootFinder).ToList();

            var dic = models.GroupBy(parentIDSelector).ToDictionary(a => a.Key, b => b.ToList());

            foreach(var rootModel in rootModels)
            {
                var tree = CreateTree(idSelector, parentIDSelector, dic, rootModel);
                forest.Add(tree);
            }

            return forest;
        }

        private static TModel CreateTree<TModel>(Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Dictionary<string, List<TModel>> tempList, TModel model)
    where TModel : TreeNode, new()
        {
            var id = idSelector(model);
            if(tempList.ContainsKey(id))
            {
                var children = tempList[id];
                tempList.Remove(idSelector(model));
                foreach(var child in children)
                {
                    var childNode = CreateTree(idSelector, parentIDSelector, tempList, child);
                    childNode.Parent = model;
                    model.Children.Add(childNode);
                }
            }

            return model;
        }






        /// <summary>
        /// 搜索树
        /// </summary>
        /// <param name="node">目标树</param>
        /// <param name="nodeContainFunc">当前节点是否符合查询条件的委托</param>
        /// <param name="childContainFunc">子节点是否符合查询条件的委托</param>
        /// <param name="isParentContainKey">当前节点的父级节点是否包含关键字</param>
        /// <returns></returns>
        public static bool SearchTreeWithVisibility(TreeNode node, Func<TreeNode, bool> nodeContainFunc, Func<TreeNode, bool> childContainFunc, bool isParentContainKey = false)
        {
            bool isContain = false;

            if(isParentContainKey)
            {
                isContain = true;
            }
            else
            {
                if(nodeContainFunc(node))
                {
                    isParentContainKey = true;
                    isContain = true;
                }
            }

            foreach(var child in node.Children)
            {
                if(child.IsLeaf)
                {
                    if(isParentContainKey || childContainFunc(child))
                    {
                        isContain = true;
                        child.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        child.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    //var childOrgEx = child as OrganizationEx;
                    var tempDelete = SearchTreeWithVisibility(child, nodeContainFunc, childContainFunc, isParentContainKey);
                    if(tempDelete)
                    {
                        isContain = true;
                    }
                }
            }

            if(isContain)
            {
                node.Visibility = Visibility.Visible;
            }
            else
            {
                node.Visibility = Visibility.Collapsed;
            }

            return isContain;
        }

        /// <summary>
        /// 搜索树
        /// </summary>
        /// <param name="node">目标树</param>
        /// <param name="nodeContainFunc">当前节点是否符合查询条件的委托</param>
        /// <param name="childContainFunc">子节点是否符合查询条件的委托</param>
        /// <param name="isParentContainKey">当前节点的父级节点是否包含关键字</param>
        /// <returns></returns>
        public static void SearchTreeWithVisibility(IEnumerable<TreeNode> nodes, Func<TreeNode, bool> nodeContainFunc, Func<TreeNode, bool> childContainFunc, bool isParentContainKey = false)
        {
            foreach(var node in nodes)
            {
                SearchTreeWithVisibility(node, nodeContainFunc, childContainFunc, isParentContainKey);
            }
        }


        public static bool SearchTreeWithDelete(TreeNode orgEx, Func<TreeNode, bool> nodeContainFunc, Func<TreeNode, bool> childContainFunc)
        {
            bool delete = true;
            //保留所有子节点
            if(nodeContainFunc(orgEx))
            {
                delete = false;
                return delete;
            }

            var needDelete = new List<TreeNode>();
            foreach(var child in orgEx.Children)
            {
                if(child.IsLeaf)
                {
                    if(childContainFunc(child))
                    {
                        delete = false;
                    }
                    else
                    {
                        needDelete.Add(child);
                    }
                }
                else
                {
                    var tempDelete = SearchTreeWithDelete(child, nodeContainFunc, childContainFunc);
                    if(tempDelete)
                    {
                        needDelete.Add(child);
                    }
                    else
                    {
                        delete = false;
                    }
                }
            }

            orgEx.Children = new ObservableCollection<TreeNode>(orgEx.Children.Except(needDelete));

            return delete;
        }
        #endregion

        #region 泛型
        /// <summary>
        /// 窗建一棵树，只有一个根节点
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <param name="idSelector"></param>
        /// <param name="parentIDSelector"></param>
        /// <returns></returns>
        public static TreeNode<TModel> CreateTreeFromModelListGeneric<TModel>(List<TModel> models, Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Func<TModel, bool> rootFinder)
            where TModel : class
        //where TResult : TreeNode<TModel>, new()
        {
            var rootModel = models.FirstOrDefault(rootFinder);

            var dic = models.Where(t => null != parentIDSelector(t)).GroupBy(parentIDSelector).ToDictionary(a => a.Key, b => b.ToList());
            var tree = CreateTreeGeneric<TModel>(idSelector, parentIDSelector, dic, rootModel);

            return tree;
        }

        /// <summary>
        /// 创建森林，多棵树，多个根节点
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <param name="idSelector"></param>
        /// <param name="parentIDSelector"></param>
        /// <returns></returns>
        public static ObservableCollection<TreeNode<TModel>> CreateTreeForestFromModelListGeneric<TModel>(List<TModel> models, Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Func<TModel, bool> rootFinder)
            where TModel : class
        //where TResult : TreeNode<TModel>, new()
        {
            var forest = new ObservableCollection<TreeNode<TModel>>();
            var rootModels = models.Where(rootFinder).ToList();

            var dic = models.GroupBy(parentIDSelector).ToDictionary(a => a.Key, b => b.ToList());

            foreach(var rootModel in rootModels)
            {
                var tree = CreateTreeGeneric<TModel>(idSelector, parentIDSelector, dic, rootModel);
                forest.Add(tree);
            }

            return forest;
        }

        private static TreeNode<TModel> CreateTreeGeneric<TModel>(Func<TModel, string> idSelector, Func<TModel, string> parentIDSelector, Dictionary<string, List<TModel>> tempList, TModel model)
            where TModel : class
        //where TResult : TreeNode<TModel>, new()
        {
            //var node = new TreeNode<TModel>(model);
            TreeNode<TModel> resultNode = new TreeNode<TModel>();
            resultNode.Model = model;

            var id = idSelector(model);
            if(tempList.ContainsKey(id))
            {
                var children = tempList[id];
                tempList.Remove(idSelector(model));
                foreach(var child in children)
                {
                    var childNode = CreateTreeGeneric<TModel>(idSelector, parentIDSelector, tempList, child);
                    childNode.Parent = resultNode;
                    resultNode.Children.Add(childNode);
                }
            }

            return resultNode;
        }

        public static bool SearchTreeWithDelete<TParent, TChild>(TreeNode<TParent> orgEx, Func<TParent, bool> nodeContainFunc, Func<TChild, bool> childContainFunc)
            where TParent : class
            where TChild : class
        {
            bool delete = true;
            //保留所有子节点
            if(nodeContainFunc(orgEx.Model))
            {
                delete = false;
                return delete;
            }

            var needDelete = new List<ITreeNode>();
            foreach(var child in orgEx.Children)
            {
                var leafChild = child as TreeNode<TChild>;
                if(leafChild != null)
                {
                    if(childContainFunc(leafChild.Model))
                    {
                        delete = false;
                    }
                    else
                    {
                        needDelete.Add(child);
                    }
                }
                else
                {
                    var tempDelete = SearchTreeWithDelete(child as TreeNode<TParent>, nodeContainFunc, childContainFunc);
                    if(tempDelete)
                    {
                        needDelete.Add(child);
                    }
                    else
                    {
                        delete = false;
                    }
                }
            }

            orgEx.Children = new ObservableCollection<ITreeNode>(orgEx.Children.Except(needDelete));

            return delete;
        }

        public static void SearchSingleTreeWithDelete<TModel>(ObservableCollection<TreeNode<TModel>> orgEx, Func<TModel, bool> nodeContainFunc)
    where TModel : class
        {
            var needDelte = new List<TreeNode<TModel>>();
            foreach(var item in orgEx)
            {
                var delete = SearchSingleTreeWithDeleteInternal(item, nodeContainFunc);
                if(delete)
                {
                    needDelte.Add(item);
                }
            }

            foreach(var item in needDelte)
            {
                orgEx.Remove(item);
            }
        }

        private static bool SearchSingleTreeWithDeleteInternal<TModel>(TreeNode<TModel> orgEx, Func<TModel, bool> nodeContainFunc)
            where TModel : class
        {
            bool delete = true;
            //保留所有子节点
            if(nodeContainFunc(orgEx.Model))
            {
                delete = false;
                return delete;
            }

            var needDelete = new List<ITreeNode>();
            foreach(var child in orgEx.Children)
            {
                var tempDelete = SearchSingleTreeWithDeleteInternal(child as TreeNode<TModel>, nodeContainFunc);
                if(tempDelete)
                {
                    needDelete.Add(child);
                }
                else
                {
                    delete = false;
                }
            }

            orgEx.Children = new ObservableCollection<ITreeNode>(orgEx.Children.Except(needDelete));

            return delete;
        }
        #endregion
    }
}
