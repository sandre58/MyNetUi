// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MyNet.UI.Busy.Models
{
    public class ProgressionBusy : Busy
    {
        public string Title { get; set; } = string.Empty;

        public IEnumerable<string>? Messages { get; set; }

        public double Value { get; set; }
    }
}
