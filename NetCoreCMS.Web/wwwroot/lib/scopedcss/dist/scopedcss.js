!function(e){if("object"==typeof exports&&"undefined"!=typeof module)module.exports=e();else if("function"==typeof define&&define.amd)define([],e);else{var f;"undefined"!=typeof window?f=window:"undefined"!=typeof global?f=global:"undefined"!=typeof self&&(f=self),f.ScopedCSS=e()}}(function(){var define,module,exports;return (function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);throw new Error("Cannot find module '"+o+"'")}var f=n[o]={exports:{}};t[o][0].call(f.exports,function(e){var n=t[o][1][e];return s(n?n:e)},f,f.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(_dereq_,module,exports){
'use strict';
var CssRule = function (cssRule, index) {
    this.rule = cssRule;
    this.index = index;
};
CssRule.prototype.formatSelector = function (prefix, selector) {
    return selector.split(', ').map(function (part) {
        return prefix + ' ' + part;
    }).join(', ');
};
CssRule.prototype.formatCssText = function (prefix, cssText) {
    var flip = 0;
    return cssText.split(/ }\s?{/).map(function (selector) {
        return ++flip % 2 ? this.formatSelector(prefix, selector) : selector;
    }, this).join(' }{');
};
CssRule.prototype.applyPrefix = function (prefix) {
    var selectorText = this.rule.selectorText;
    var parentStyleSheet = this.rule.parentStyleSheet;
    var cssText = this.rule.cssText;
    selectorText = selectorText ? selectorText.replace(/\"/g, '\'') : '';
    if (selectorText.indexOf(prefix) !== 0) {
        this.rule.selectorText = this.formatSelector(prefix, selectorText);
        if (this.rule.selectorText === selectorText) {
            cssText = this.formatCssText(prefix, cssText);
            parentStyleSheet.deleteRule(this.index);
            parentStyleSheet.insertRule(cssText, this.index);
        }
    }
};
module.exports = CssRule;
},{}],2:[function(_dereq_,module,exports){
var StyleSheet = _dereq_('./styleSheet');
'use strict';
var ScopedCss = function (prefix, cssText, styleTag) {
    this.prefix = prefix;
    this.cssText = cssText;
    this.styleTag = styleTag || document.createElement('style');
};
ScopedCss.defaultSelector = ':not([data-scopedcss]) style[scoped]';
ScopedCss.VERSION = '0.1.4';
ScopedCss.prototype.prepare = function (cssText) {
    return cssText.replace(/:host/g, this.prefix);
};
ScopedCss.prototype.process = function (prefix) {
    if (prefix) {
        this.prefix = prefix;
    }
    var cssText = this.cssText || this.styleTag.innerHTML;
    var styleText = document.createTextNode(this.prepare(cssText));
    this.styleTag.innerHTML = '';
    this.styleTag.appendChild(styleText);
    var styleSheet = new StyleSheet(this.styleTag);
    var cssRules = styleSheet.cssRules();
    if (this.prefix) {
        cssRules.forEach(function (rule) {
            rule.applyPrefix(this.prefix);
        }, this);
    }
};
ScopedCss.applyTo = function (hostElement) {
    hostElement = hostElement || document.body;
    var elements = hostElement.querySelectorAll(this.defaultSelector);
    Array.prototype.slice.call(elements).forEach(function (element) {
        var id = (new Date() * Math.random()).toString(16);
        element.parentNode.setAttribute('data-scopedcss', id);
        new ScopedCss('[data-scopedcss=\'' + id + '\']', null, element).process();
    });
};
module.exports = ScopedCss;
},{"./styleSheet":3}],3:[function(_dereq_,module,exports){
var CssRule = _dereq_('./cssRule');
'use strict';
var StyleSheet = function (styleTag) {
    this.styleTag = styleTag;
    if (!this.styleTag.parentNode) {
        document.head.appendChild(this.styleTag);
    }
};
StyleSheet.prototype.getStyleSheet = function () {
    return this.styleTag.sheet;
};
StyleSheet.prototype.cssRules = function () {
    var rules = Array.prototype.slice.call(this.getStyleSheet().cssRules);
    return rules.map(function (rule, index) {
        return new CssRule(rule, index);
    });
};
module.exports = StyleSheet;
},{"./cssRule":1}]},{},[2])
(2)
});