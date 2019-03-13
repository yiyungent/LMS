using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;

namespace Webs.Extensions
{
    public static class HtmlHelperExtRBL
    {
        /// <summary>
        /// eg: <input type="radio" name="sex" value="0" title="男" checked="checked">
        /// </summary>
        public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, bool useTitle, object htmlAttributes)
        {
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SelectListItem selectItem in selectList)
            {
                IDictionary<string, object> newHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                index++;
                newHtmlAttributes.Add("id", name + "_" + index.ToString());
                newHtmlAttributes.Add("name", name);
                newHtmlAttributes.Add("type", "radio");
                newHtmlAttributes.Add("value", selectItem.Value);
                newHtmlAttributes.Add("title", selectItem.Text);
                if (selectItem.Selected)
                {
                    newHtmlAttributes.Add("checked", "checked");
                }
                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttributes<string, object>(newHtmlAttributes);
                string inputAllHtml = tagBuilder.ToString(TagRenderMode.SelfClosing);
                stringBuilder.Append(inputAllHtml);
            }
            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, bool useTitle)
        {
            return RadioButtonList(helper, name, selectList, useTitle, new { });
        }
    }
}