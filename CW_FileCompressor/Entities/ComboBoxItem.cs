using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_FileCompressor.Entities
{
    /// <summary>
    /// The class represents ComboBox option. 
    /// Used for dynamic creation.
    /// </summary>
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
