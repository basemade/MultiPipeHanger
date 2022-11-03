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


    #region(執行外掛後點選取多個物件的方法_1)
    /*
    public class Test02_PickMultiObjects : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            StringBuilder st = new StringBuilder();

            

            //IList<Element> elems = sel.PickObjects(ObjectType.Element, "選擇牆");
            //上面這行報錯，因為方法 PickObject() 回傳型別為reference
            IList<Reference> references = sel.PickObjects(ObjectType.Element, "選擇要加入的管");


            //IList<Element> elems = doc.GetElement(references);
            //上面這行報錯，因為方法 GetElement() 回傳型別為reference(單數)


            List<Element> elems = new List<Element>();
            foreach (Reference refer in references)
            {
                Element elem = doc.GetElement(refer);
                st.AppendLine("elem.Id：" + elem.Id);
                elems.Add(elem);
            }
            //references並無點選排序
            //上面這種作法雖然可行，但...?
            st.AppendLine($"references數量為：{references.Count}");
            st.AppendLine($"elems數量為：{elems.Count}");

            
            MessageBox.Show(st.ToString());
            return Result.Succeeded;
        }
    */
    #endregion

    #region(執行外掛後點選取多個物件的方法_2) 
    public class Test02_PickMultiObjects : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            StringBuilder st = new StringBuilder();

            ISelectionFilter gagaFilter = new SelectElementsFilter(doc);
            //List<Reference> selPipeRefs = sel.PickObjects(ObjectType.Element, gagaFilter, "選擇要加入的管").ToList();
            IList<Reference> selPipeRefs = sel.PickObjects(ObjectType.Element, gagaFilter, "選擇要加入的管");
            foreach (Reference refer in selPipeRefs)
            {
                Element e = doc.GetElement(refer);
                st.AppendLine("element.Id：" + e.Id);
            }


            MessageBox.Show(st.ToString());
            return Result.Succeeded;
        }
        public class SelectElementsFilter : ISelectionFilter
        {
            Document docDefault = null;
            public SelectElementsFilter(Document doc)
            {
                docDefault = doc;
            }

            public bool AllowElement(Element eForFil)
            {
                if (eForFil.Category.Id.IntegerValue == BuiltInCategory.OST_PipeCurves.GetHashCode())
                //if (eForFil.Category.Id == Category.GetCategory(docDefault, BuiltInCategory.OST_PipeCurves).Id)
                //上面這行可以運作，但其實不用再透過文件去撈
                    return true;
                else
                    return false;
            }

            public bool AllowReference(Reference r, XYZ p)
            {
                return true;
            }
        }



    }
    #endregion
}
