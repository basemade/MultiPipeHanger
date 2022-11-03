using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Test_01_03
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Test01_PickObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection selection = uidoc.Selection;
            //Element element = selection.PickObject(ObjectType.Element);
            //上面這行會報錯，因為方法 PickObject() 回傳型別為reference
            Reference reference = selection.PickObject(ObjectType.Element);

            StringBuilder st = new StringBuilder();

            if (reference != null)
            {
                st.AppendLine("管件已被選取");

                Element element = doc.GetElement(reference);
                st.AppendLine("element.Id：" + element.Id);
                
            }

            
            MessageBox.Show(st.ToString());
            return Result.Succeeded;
        }
    }
}
