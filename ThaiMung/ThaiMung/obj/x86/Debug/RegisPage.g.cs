﻿

#pragma checksum "C:\Users\parat_000\documents\visual studio 2012\Projects\ThaiMung\ThaiMung\RegisPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "19FF9FDC5A3C493E58D9FA1207EEED2C"
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
    partial class RegisPage : global::ThaiMung.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 50 "..\..\..\RegisPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.submit_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 51 "..\..\..\RegisPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cancel_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


