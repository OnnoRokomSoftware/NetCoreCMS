define(function() {
  "use strict";

  /**
   * Wraps a native CSSRule object with methods that can augment its internal
   * selector.
   *
   * Examples:
   *
   * var cssRule = new CssRule(rule, 0);
   *
   * @param  {String} cssRule Native CSSRule object.
   * @param  {String} index What position the rule is at.
   */
  var CssRule = function(cssRule, index) {
    this.rule = cssRule;
    this.index = index;
  };

  /**
   * Breaks a selector down to its unique selectorText.
   *
   * Examples:
   *
   * cssRule.formatSelector(".prefix", "h1, h2 > *.red");
   *
   * @param  {String} prefix Unique selector to prefix to the unscoped
   *                  selector.
   * @param  {String} selector May contain one or many compound parts.
   * @return {String} The prefixed selector.
   */
  CssRule.prototype.formatSelector = function(prefix, selector) {
    return selector.split(", ").map(function(part) {
      return prefix + " " + part;
    }).join(", ");
  };

  /**
   * Browsers that incorrectly support the mutable selectorText property will
   * have to pass through this code path.  This method takes in the CSSRule
   * `cssText` property and augments each selector separately.
   *
   * Examples:
   *
   * cssRule.formatCssText(".prefix", "h1, h2 > *.red { color: red; }");
   *
   * @param  {String} prefix Unique selector to prefix to the unscoped
   *                  selector.
   * @param  {String} cssText Full from the CSSRule.
   * @return {String} The prefixed cssText.
   */
  CssRule.prototype.formatCssText = function(prefix, cssText) {
    var flip = 0;

    // Parse out all the selector rules and update using `formatSelector`.
    // The regular expression that splits on all opening { that are preceeded
    // by a closing }.
    return cssText.split(/ }\s?{/).map(function(selector) {
      // Only modify every other item.
      return (++flip % 2) ? this.formatSelector(prefix, selector) : selector;
    }, this).join(" }{");
  };

  /**
   * Facilitates the selectorText prefixing.  Pass in a prefix and the
   * internal structure will be modified.  Requires the CSSStyleSheet APIs.
   *
   * Description:
   *
   * Many browsers do not support directly modifying the `selectorText`
   * property.  The specified behavior is that the property should be mutable.
   *
   * Examples:
   *
   * cssRule.applyPrefix(".prefix");
   *
   * @param  {String} prefix Unique selector to prefix to the unscoped
   *                  selector.
   */
  CssRule.prototype.applyPrefix = function(prefix) {
    var selectorText = this.rule.selectorText;
    var parentStyleSheet = this.rule.parentStyleSheet;
    var cssText = this.rule.cssText;

    // Coerce to single quotes, or use an empty string.
    selectorText = selectorText ? selectorText.replace(/\"/g, "'") : "";

    // Don't scope if it's the same selector.
    if (selectorText.indexOf(prefix) !== 0) {
      // Attempt to directly modify the selector.
      this.rule.selectorText = this.formatSelector(prefix, selectorText);

      // The specification actually marks the above property to be mutable, but
      // only Chrome appears to implement it.  Below is a fallback that should
      // work in most browsers that support the StyleSheet API correctly.
      if (this.rule.selectorText === selectorText) {
        // Update the CSS text to account for the prefix.
        cssText = this.formatCssText(prefix, cssText);

        // Swap out the rule with the modified cssText.
        parentStyleSheet.deleteRule(this.index);
        parentStyleSheet.insertRule(cssText, this.index);
      }
    }
  };

  return CssRule;
});
