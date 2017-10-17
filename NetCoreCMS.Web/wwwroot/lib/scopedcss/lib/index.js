define(["./styleSheet"], function(StyleSheet) {
  "use strict";

  /**
   * The global construct for assembling scoped style sheets.  This shadows
   * the functionality within the library in a more convenient API.
   *
   * Examples:
   *
   * // Without passing a style tag.
   * var scopedCss = new ScopedCss(".prefix", "h1 { color: red; }");
   *
   * // Passing along your own styleTag element.
   * var scopedCss = new ScopedCss(".prefix", "h1 { color: red; }", styleTag);
   *
   * // Passing along only the prefix.
   * var scopedCss = new ScopedCss(".prefix");
   * // And assigning the cssText afterwards.
   * scopedCss.cssText = "h1 { color: red; }";
   *
   * @param  {String} prefix Unique selector to prefix to the unscoped
   *                  selector.
   * @param  {String} cssText Content to populate the `<style>` tag with.
   * @param  {HTMLStyleElement} styleTag Element containing styles to prefix.
   */
  var ScopedCss = function(prefix, cssText, styleTag) {
    this.prefix = prefix;
    this.cssText = cssText;

    // Default to an internal `<style>` tag if one wasn't passed.
    this.styleTag = styleTag || document.createElement("style");
  };

  // This is the default selector to look for when monitoring.
  ScopedCss.defaultSelector = ":not([data-scopedcss]) style[scoped]";

  // Attach the version information.
  ScopedCss.VERSION = "0.1.4";

  /**
   * This method replaces all instances of the host keyword with the passed in
   * prefix.
   *
   * Examples:
   *
   * scopedCss.prepare(":host { color: blue; }");
   *
   * @param  {String} cssText Derived from the style element `innerHTML`.
   * @return {String} Text prepared to be injected with `innerHTML`.
   */
  ScopedCss.prototype.prepare = function(cssText) {
    // Replace `:host` with the prefix.
    return cssText.replace(/:host/g, this.prefix);
  };

  /**
   * If the styleTag is empty this method will fill it with the prepared
   * cssText and apply the prefix to every rules.
   *
   * Examples:
   *
   * // Optionally pass along a prefix.
   * scopedCss.process(".prefix");
   *
   * @param  {String} prefix Optionally specify a prefix to use; this is useful
   *                  in preloaders who wish to expose a ScopedCss object and
   *                  not enforce setting the prefix property directly.
   */
  ScopedCss.prototype.process = function(prefix) {
    // If a prefix override was specified, reset it.
    if (prefix) {
      this.prefix = prefix;
    }

    var cssText = this.cssText || this.styleTag.innerHTML;
    var styleText = document.createTextNode(this.prepare(cssText));

    this.styleTag.innerHTML = "";
    this.styleTag.appendChild(styleText);

    var styleSheet = new StyleSheet(this.styleTag);
    var cssRules = styleSheet.cssRules();

    // Only mess with the CSS rules if a prefix was specified.
    if (this.prefix) {
      cssRules.forEach(function(rule) {
        rule.applyPrefix(this.prefix);
      }, this);
    }
  };

  /**
   * Static method that processes a given element for any selectors that match
   * the default selector.  The default selector will find any style elements
   * that haven't already been prefixed and contain the scoped attribute.
   *
   * Examples:
   *
   * // Find all scoped style sheets under the body tag.
   * ScopedCss.applyTo(document.body);
   *
   * @param  {String} hostElement What element to find descendent scoped style
   *                  tags in.
   */
  ScopedCss.applyTo = function(hostElement) {
    // Default to the body element.
    hostElement = hostElement || document.body;

    // Query for all the scoped style tags that have not already been
    // processed, scope to the `hostElement`.
    var elements = hostElement.querySelectorAll(this.defaultSelector);

    // Coerce to an Array and iterate through each matched element.
    Array.prototype.slice.call(elements).forEach(function(element) {
      // Create a custom identifier for this element, since scoped doesn't
      // actually exist yet.
      var id = (new Date() * Math.random()).toString(16);
      element.parentNode.setAttribute("data-scopedcss", id);

      // Create a new scoped stylesheet that we will replace the existing with.
      new ScopedCss("[data-scopedcss='" + id + "']", null, element).process();
    });
  };

  return ScopedCss;
});
