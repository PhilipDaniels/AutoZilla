using AutoZilla.Core;
using AutoZilla.Core.Extensions;
using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Plugins.MSSQL
{
    public class MSSQLPlugin : IAutoZillaPlugin
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IGlobalHotKeyManager HKM;
        static TextOutputter TOUT = new TextOutputter();


        public void InitialiseHotKeyManager(IGlobalHotKeyManager manager)
        {
            HKM = manager;
            log.Debug("Initialising.");

            RegisterKeys();
        }

        void RegisterKeys()
        {
            HKM.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.C, OutputCommentHeader);
            HKM.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.S, OutputSelectTop10Star);
            HKM.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.D, OutputSelectCountStar);
            HKM.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.F, OutputSelectStar);
            HKM.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.U, OutputReadUncommitted);
        }

        static void OutputCommentHeader(ModifiedKey key)
        {
            var str = @"
/* Summary
 *
 *
 * Example Usage
 * EXEC 
 *
 * Date       Author              Description
 * ${DATE:dd-MM-yyyy} ${USER!20}Initial version.
 */
".Templatize();

            TOUT.WaitForModifiersUp();
            TOUT.PasteString(str).
                Move(-9, 3).
                SelectToEndOfLine();
        }

        static void OutputSelectTop10Star(ModifiedKey key)
        {
            TOUT.WaitForModifiersUp();
            TOUT.PasteString("SELECT TOP 10 * FROM ");
        }

        static void OutputSelectStar(ModifiedKey key)
        {
            TOUT.WaitForModifiersUp();
            TOUT.PasteString("SELECT * FROM ");
        }

        static void OutputSelectCountStar(ModifiedKey key)
        {
            TOUT.WaitForModifiersUp();
            TOUT.PasteString("SELECT COUNT(*) FROM ");
        }

        static void OutputReadUncommitted(ModifiedKey key)
        {
            TOUT.WaitForModifiersUp();
            TOUT.PasteLine("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" + Environment.NewLine);
        }
    }
}
