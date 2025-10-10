using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.UI;
public interface ISnackbarService
{
    void Show(string message, int duration = 2000);
}
