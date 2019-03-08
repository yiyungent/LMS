using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;

public static class RadioButtonListHelper
{
    public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
    {
        return RadioButtonList(helper, name, selectList, new { });
    }
    public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
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
            if (selectItem.Selected)
            {
                newHtmlAttributes.Add("checked", "checked");
            }
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes<string, object>(newHtmlAttributes);
            string inputAllHtml = tagBuilder.ToString(TagRenderMode.SelfClosing);
            stringBuilder.AppendFormat(@"{0}{1}&nbsp;&nbsp;&nbsp;&nbsp;", inputAllHtml, selectItem.Text);
        }
        return MvcHtmlString.Create(stringBuilder.ToString());
    }
}