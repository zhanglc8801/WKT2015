﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.586
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="JournalXServiceSoap", Namespace="http://tempuri.org/")]
public partial class JournalXService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback AddSchemaOperationCompleted;
    
    private System.Threading.SendOrPostCallback UpdateSchemaOperationCompleted;
    
    private System.Threading.SendOrPostCallback DeleteSchemaOperationCompleted;
    
    private System.Threading.SendOrPostCallback GetFFXTemplateOperationCompleted;
    
    private System.Threading.SendOrPostCallback MessageNotificationOperationCompleted;
    
    /// <remarks/>
    public JournalXService() {
        this.Url = "http://172.19.70.3/Service/JournalXService.asmx";
    }
    
    /// <remarks/>
    public event AddSchemaCompletedEventHandler AddSchemaCompleted;
    
    /// <remarks/>
    public event UpdateSchemaCompletedEventHandler UpdateSchemaCompleted;
    
    /// <remarks/>
    public event DeleteSchemaCompletedEventHandler DeleteSchemaCompleted;
    
    /// <remarks/>
    public event GetFFXTemplateCompletedEventHandler GetFFXTemplateCompleted;
    
    /// <remarks/>
    public event MessageNotificationCompletedEventHandler MessageNotificationCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AddSchema", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int AddSchema(string zipFilePah) {
        object[] results = this.Invoke("AddSchema", new object[] {
                    zipFilePah});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginAddSchema(string zipFilePah, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("AddSchema", new object[] {
                    zipFilePah}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndAddSchema(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void AddSchemaAsync(string zipFilePah) {
        this.AddSchemaAsync(zipFilePah, null);
    }
    
    /// <remarks/>
    public void AddSchemaAsync(string zipFilePah, object userState) {
        if ((this.AddSchemaOperationCompleted == null)) {
            this.AddSchemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddSchemaOperationCompleted);
        }
        this.InvokeAsync("AddSchema", new object[] {
                    zipFilePah}, this.AddSchemaOperationCompleted, userState);
    }
    
    private void OnAddSchemaOperationCompleted(object arg) {
        if ((this.AddSchemaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.AddSchemaCompleted(this, new AddSchemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateSchema", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int UpdateSchema(int id, string zipFilePath) {
        object[] results = this.Invoke("UpdateSchema", new object[] {
                    id,
                    zipFilePath});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginUpdateSchema(int id, string zipFilePath, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UpdateSchema", new object[] {
                    id,
                    zipFilePath}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndUpdateSchema(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void UpdateSchemaAsync(int id, string zipFilePath) {
        this.UpdateSchemaAsync(id, zipFilePath, null);
    }
    
    /// <remarks/>
    public void UpdateSchemaAsync(int id, string zipFilePath, object userState) {
        if ((this.UpdateSchemaOperationCompleted == null)) {
            this.UpdateSchemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateSchemaOperationCompleted);
        }
        this.InvokeAsync("UpdateSchema", new object[] {
                    id,
                    zipFilePath}, this.UpdateSchemaOperationCompleted, userState);
    }
    
    private void OnUpdateSchemaOperationCompleted(object arg) {
        if ((this.UpdateSchemaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.UpdateSchemaCompleted(this, new UpdateSchemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeleteSchema", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int DeleteSchema(int id) {
        object[] results = this.Invoke("DeleteSchema", new object[] {
                    id});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginDeleteSchema(int id, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("DeleteSchema", new object[] {
                    id}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndDeleteSchema(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void DeleteSchemaAsync(int id) {
        this.DeleteSchemaAsync(id, null);
    }
    
    /// <remarks/>
    public void DeleteSchemaAsync(int id, object userState) {
        if ((this.DeleteSchemaOperationCompleted == null)) {
            this.DeleteSchemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteSchemaOperationCompleted);
        }
        this.InvokeAsync("DeleteSchema", new object[] {
                    id}, this.DeleteSchemaOperationCompleted, userState);
    }
    
    private void OnDeleteSchemaOperationCompleted(object arg) {
        if ((this.DeleteSchemaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.DeleteSchemaCompleted(this, new DeleteSchemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetFFXTemplate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string GetFFXTemplate(int markId) {
        object[] results = this.Invoke("GetFFXTemplate", new object[] {
                    markId});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetFFXTemplate(int markId, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetFFXTemplate", new object[] {
                    markId}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndGetFFXTemplate(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void GetFFXTemplateAsync(int markId) {
        this.GetFFXTemplateAsync(markId, null);
    }
    
    /// <remarks/>
    public void GetFFXTemplateAsync(int markId, object userState) {
        if ((this.GetFFXTemplateOperationCompleted == null)) {
            this.GetFFXTemplateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFFXTemplateOperationCompleted);
        }
        this.InvokeAsync("GetFFXTemplate", new object[] {
                    markId}, this.GetFFXTemplateOperationCompleted, userState);
    }
    
    private void OnGetFFXTemplateOperationCompleted(object arg) {
        if ((this.GetFFXTemplateCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.GetFFXTemplateCompleted(this, new GetFFXTemplateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/MessageNotification", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string MessageNotification(string strSafetyCode, string MsgType, string MessageParameter) {
        object[] results = this.Invoke("MessageNotification", new object[] {
                    strSafetyCode,
                    MsgType,
                    MessageParameter});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginMessageNotification(string strSafetyCode, string MsgType, string MessageParameter, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("MessageNotification", new object[] {
                    strSafetyCode,
                    MsgType,
                    MessageParameter}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndMessageNotification(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void MessageNotificationAsync(string strSafetyCode, string MsgType, string MessageParameter) {
        this.MessageNotificationAsync(strSafetyCode, MsgType, MessageParameter, null);
    }
    
    /// <remarks/>
    public void MessageNotificationAsync(string strSafetyCode, string MsgType, string MessageParameter, object userState) {
        if ((this.MessageNotificationOperationCompleted == null)) {
            this.MessageNotificationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMessageNotificationOperationCompleted);
        }
        this.InvokeAsync("MessageNotification", new object[] {
                    strSafetyCode,
                    MsgType,
                    MessageParameter}, this.MessageNotificationOperationCompleted, userState);
    }
    
    private void OnMessageNotificationOperationCompleted(object arg) {
        if ((this.MessageNotificationCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.MessageNotificationCompleted(this, new MessageNotificationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void AddSchemaCompletedEventHandler(object sender, AddSchemaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class AddSchemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal AddSchemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void UpdateSchemaCompletedEventHandler(object sender, UpdateSchemaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class UpdateSchemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal UpdateSchemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void DeleteSchemaCompletedEventHandler(object sender, DeleteSchemaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class DeleteSchemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal DeleteSchemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void GetFFXTemplateCompletedEventHandler(object sender, GetFFXTemplateCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class GetFFXTemplateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal GetFFXTemplateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public string Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void MessageNotificationCompletedEventHandler(object sender, MessageNotificationCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class MessageNotificationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal MessageNotificationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public string Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[0]));
        }
    }
}
