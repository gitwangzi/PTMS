using Jounce.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;

namespace BaseLib.SystemFunction
{
    public class FunctionItem : BaseNotify
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Version { get; set; }
        public string Module { get; set; }

        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }
        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;

                if (HasChildren)
                {
                    Children.ToList().ForEach(t => t.UpdateCheckStatus(NotifyDirection.Down));
                }

                if (value && Depends.Count > 0)
                {
                    Depends.ToList().ForEach(e => e.UpdateCheckStatus(NotifyDirection.Horizontal));
                }

                if (Parent != null)
                {
                    Parent.UpdateCheckStatus(NotifyDirection.Up);

                    if (value == false)
                    {
                        foreach (var borther in Parent.Children)
                        {
                            if (borther == this)
                            {
                                continue;
                            }

                            if (borther.Depends.Any(t => t.ID == this.ID))
                            {
                                if (borther.IsChecked)
                                {
                                    borther.UpdateDependStatus();
                                }
                            }
                        }
                    }
                }

                RaisePropertyChanged("IsChecked");
            }
        }

        protected void UpdateDependStatus()
        {
            this.isChecked = false;
            RaisePropertyChanged("IsChecked");
            Parent.UpdateCheckStatus(NotifyDirection.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">通知方向</param>
        protected void UpdateCheckStatus(NotifyDirection direction)
        {
            if (direction == NotifyDirection.Up)
            {
                this.isChecked = this.Children.Count(t => t.IsChecked) > 0;

                if (Parent != null)
                {
                    Parent.UpdateCheckStatus(direction);
                }
            }
            else if (direction == NotifyDirection.Down)
            {
                this.isChecked = this.Parent.IsChecked;

                if (HasChildren)
                {
                    Children.ToList().ForEach(t => t.UpdateCheckStatus(direction));
                }
            }
            else if (direction == NotifyDirection.Horizontal)
            {
                this.isChecked = true;
            }

            RaisePropertyChanged("IsChecked");
        }

        public FunctionItem Parent { get; set; }

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

        public ObservableCollection<FunctionItem> Children { get; set; }

        public List<FunctionItem> Depends { get; set; }

        public FunctionItem()
        {
            Children = new ObservableCollection<FunctionItem>();
            Depends = new List<FunctionItem>();
        }

        protected enum NotifyDirection
        {
            /// <summary>
            /// 向上通知
            /// </summary>
            Up = 1,
            Down,
            Horizontal
        }
    }
}
