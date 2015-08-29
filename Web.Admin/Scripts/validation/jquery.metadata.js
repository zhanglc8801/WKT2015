/*
 * Metadata - jQuery plugin for parsing metadata from elements
 *
 * Copyright (c) 2006 John Resig, Yehuda Katz, J�örn Zaefferer, Paul McLanahan
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 *
 * Revision: $Id: jquery.metadata.js 4187 2007-12-16 17:15:27Z joern.zaefferer $
 *
 */

(function($) {

$.extend({
	metadata : {
		defaults : {
			type: 'class',
			name: 'metadata',
			cre: /({.*})/,
			single: 'metadata'
		},
		setType: function( type, name ){
			this.defaults.type = type;
			this.defaults.name = name;
		},
		get: function( elem, opts ){
			var settings = $.extend({},this.defaults,opts);
			// check for empty string in single property
			if ( !settings.single.length ) settings.single = 'metadata';
			
			var data = $.data(elem, settings.single);
			// returned cached data if it already exists
			if ( data ) return data;
			
			data = "{}";
			
			if ( settings.type == "class" ) {
				var m = settings.cre.exec( elem.className );
				if ( m )
					data = m[1];
			} else if ( settings.type == "elem" ) {
				if( !elem.getElementsByTagName )
					return undefined;
				var e = elem.getElementsByTagName(settings.name);
				if ( e.length )
					data = $.trim(e[0].innerHTML);
			} else if ( elem.getAttribute != undefined ) {
				var attr = elem.getAttribute( settings.name );
				if ( attr )
					data = attr;
			}
			
			if ( data.indexOf( '{' ) <0 )
			data = "{" + data + "}";
			
			data = eval("(" + data + ")");
			
			$.data( elem, settings.single, data );
			return data;
		}
	}
});

/**
 * Returns the metadata object for the first member of the jQuery object.
 *
 * @name metadata
 * @descr Returns element's metadata object
 * @param Object opts An object contianing settings to override the defaults
 * @type jQuery
 * @cat Plugins/Metadata
 */
$.fn.metadata = function( opts ){
	return $.metadata.get( this[0], opts );
};

})(jQuery);

// 手机号码验证
jQuery.validator.addMethod("mobile", function (value, element) {
    var length = value.length;
    var mobile = /^(13|14|15|17|18)[0-9]{9}$/;
    return this.optional(element) || (length == 11 && mobile.test(value));
}, "请输入正确的手机号码");
// 电话号码验证   
jQuery.validator.addMethod("phone", function (value, element) {
    var tel = /^\d{3,4}-\d{7,8}(-\d{3,4})?$/;
    return this.optional(element) || (tel.test(value));
}, "电话号码格式错误");

// 邮政编码验证   
jQuery.validator.addMethod("zipCode", function (value, element) {
    var tel = /^[0-9]{6}$/;
    return this.optional(element) || (tel.test(value));
}, "邮政编码格式错误");

// QQ号码验证   
jQuery.validator.addMethod("qq", function (value, element) {
    var tel = /^[1-9]\d{4,9}$/;
    return this.optional(element) || (tel.test(value));
}, "qq号码格式错误");

// IP地址验证
jQuery.validator.addMethod("ip", function (value, element) {
    var ip = /^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    return this.optional(element) || (ip.test(value) && (RegExp.$1 < 256 && RegExp.$2 < 256 && RegExp.$3 < 256 && RegExp.$4 < 256));
}, "Ip地址格式错误");
// 字节长度验证
jQuery.validator.addMethod("byteRangeLength", function (value, element, param) {
    var length = value.length;
    for (var i = 0; i < value.length; i++) {
        if (value.charCodeAt(i) > 127) {
            length++;
        }
    }
    return this.optional(element) || (length >= param[0] && length <= param[1]);
}, $.validator.format("请确保输入的值在{0}-{1}个字节之间(一个中文字算2个字节)"));
// 字节长度验证
jQuery.validator.addMethod("byteMaxLength", function (value, element, param) {
    var length = value.length;
    for (var i = 0; i < value.length; i++) {
        if (value.charCodeAt(i) > 127) {
            length++;
        }
    }
    return this.optional(element) || length <= param;
}, $.validator.format("请输入一个字节长度最多是{0}的字符串(一个中文字算2个字节)"));
//电话验证
jQuery.validator.addMethod("Tel", function (value, element) {
    var length = value.length;
    var mobile = /^(13|15|18|14|17)[0-9]{9}$/;
    var tel = /^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$/;
    return this.optional(element) || ((length == 11 && mobile.test(value)) || tel.test(value));
}, "请输入正确的手机号码或电话号码");
// 金额验证
jQuery.validator.addMethod("money", function (value, element) {
    var length = value.length;
    var money = /^-?(([1-9]+[0-9]*)|[0]{1})(\.[0-9]+)?$/;
    return this.optional(element) || (money.test(value));
}, "请输入正确的金额");
// 正金额验证
jQuery.validator.addMethod("positivemoney", function (value, element) {
    var length = value.length;
    var money = /^-?(([1-9]+[0-9]*)|[0]{1})(\.[0-9]+)?$/;
    return this.optional(element) || (money.test(value) && value > 0);
}, "请输入大于0的金额");