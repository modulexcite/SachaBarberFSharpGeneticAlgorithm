using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BioCSharp.Controls
{
   

    public class BiomorphItemsControl : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is BiomorphItem);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BiomorphItem();
        }
    }
}
