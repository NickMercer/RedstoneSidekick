using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RedstoneSidekickWPF.Converters
{
    public class RemainingCountToIngredientsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ingredients = (IEnumerable<ICraftingTreeItem>)values[0];
            var currentAmount = (int)values[1];
            var requiredAmount = (int)values[2];

            if(currentAmount == requiredAmount)
            {
                return null;
            }
            else
            {
                return ingredients;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
