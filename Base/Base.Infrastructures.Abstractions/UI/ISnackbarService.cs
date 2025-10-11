using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.UI;
public interface ISnackbarService
{
    void Show(string message, int duration = 2000);
}
