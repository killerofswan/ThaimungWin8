﻿

#pragma checksum "C:\Users\parat_000\documents\visual studio 2012\Projects\ThaiMung\ThaiMung\ViewPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1751C0737953DF8B7C5A9E9C2AA2BC45"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThaiMung
{
    partial class ViewPage : global::ThaiMung.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 77 "..\..\..\ViewPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.canceleditButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 46 "..\..\..\ViewPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 48 "..\..\..\ViewPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.edit_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 49 "..\..\..\ViewPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.deleteViewButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


