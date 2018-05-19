using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cells_db.util
{
    class DialogManager
    {
        public static void showDialogError(String text)
        {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void showDialogInfo(String text)
        {
            MessageBox.Show(text, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool showDialogYesNo(String text)
        {
            if (MessageBox.Show(text, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                return true;
            else return false;

        }
    }
}
