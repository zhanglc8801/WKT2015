jQuery.fn.pagination = function (maxentries, opts) {
    opts = jQuery.extend({
        items_per_page: 10,
        num_display_entries: 10,
        current_page: 0,
        num_edge_entries: 0,
        link_to: "javascript:void(0);",
        prev_text: "上一页",
        next_text: "下一页",
        ellipse_text: "...",
        prev_show_always: true,
        next_show_always: true,
        impcallback: true,
        showtotalinfo: true,
        showhome: true,
        showend: true,
        showjump: true,
        callback: function () { return false; }
    }, opts || {});

    return this.each(function () {
        /**
        * Calculate the maximum number of pages
        */
        function numPages() {
            return Math.ceil(maxentries / opts.items_per_page);
        }

        function getInterval() {
            var ne_half = Math.ceil(opts.num_display_entries / 2);
            var np = numPages();
            var upper_limit = np - opts.num_display_entries;
            var start = current_page > ne_half ? Math.max(Math.min(current_page - ne_half, upper_limit), 0) : 0;
            var end = current_page > ne_half ? Math.min(current_page + ne_half, np) : Math.min(opts.num_display_entries, np);
            return [start, end];
        }

        function pageSelected(page_id, evt) {
            current_page = page_id;
            drawLinks();
            var continuePropagation = opts.callback(page_id, panel);
            if (!continuePropagation) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                }
                else {
                    evt.cancelBubble = true;
                }
            }
            return continuePropagation;
        }

        function GotoBtn(evt) {
            var inputnum = '';
            $('input[name="pagingNum"]').each(
				function () {
				    if ($(this).val() != "") {
				        inputnum = $(this).val();
				        return false;
				    }
				}
			)
            if (inputnum == "") {
                alert('请输入页码');
                return false;
            }
            var reNum = /^[0-9]*[1-9][0-9]*$/;
            if (!reNum.test(inputnum)) {
                alert("请输入正确的页码");
                return false;
            }
            var curpage = parseInt(inputnum) - 1;
            if (curpage == current_page)
                return;
            var np = numPages();
            if (curpage >= np || curpage < 0) {
                alert('请输入正确的页码');
                return false;
            }
            pageSelected(curpage, evt);
            return true;
        }

        function drawLinks() {
            panel.empty();
            var np = numPages();
            if (np <= 1) {
                return;
            }
            if (opts.showtotalinfo) {
                panel.append(J.FormatString('<div class="msg">共{0}条记录，当前第{1}/{2}，每页{3}条记录</div>', maxentries, (current_page + 1), np, opts.items_per_page));
            }
            var interval = getInterval();
            // This helper function returns a handler function that calls pageSelected with the right page_id
            var getClickHandler = function (page_id) {
                return function (evt) { return pageSelected(page_id, evt); }
            }
            // Helper function for generating a single link (or a span tag if it'S the current page)
            var appendItem = function (page_id, appendopts) {
                page_id = page_id < 0 ? 0 : (page_id < np ? page_id : np - 1); // Normalize page id to sane value
                appendopts = jQuery.extend({ text: page_id + 1, classes: "" }, appendopts || {});
                if (page_id == current_page) {
                    var lnk = $("<a class=\"button-white\" style=\"filter:Alpha(Opacity=60);opacity:0.6;\" href=\"javascript:void(0);\"><span> " + (appendopts.text) + " </span></a>");
                }
                else {
                    var lnk = $("<a class=\"button-white\"><span> " + (appendopts.text) + " </span></a>")
						.bind("click", getClickHandler(page_id))
						.attr('href', opts.link_to.replace(/__id__/, page_id));


                }
                if (appendopts.classes) { lnk.addClass(appendopts.classes); }
                panel.append(lnk);
            }
            // 首页
            if (opts.showhome) {
                appendItem(0, { text: '首页', classes: "" });
            }
            // Generate "Previous"-Link
            if (opts.prev_text && (current_page > 0 || opts.prev_show_always)) {
                appendItem(current_page - 1, { text: opts.prev_text, classes: "prev" });
            }

            // Generate starting points
            if (interval[0] > 0 && opts.num_edge_entries > 0) {
                var end = Math.min(opts.num_edge_entries, interval[0]);
                for (var i = 0; i < end; i++) {
                    appendItem(i);
                }
                if (opts.num_edge_entries < interval[0] && opts.ellipse_text) {
                    jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
            }
            // Generate interval links
            for (var i = interval[0]; i < interval[1]; i++) {
                appendItem(i);
            }
            // Generate ending points
            if (interval[1] < np && opts.num_edge_entries > 0) {
                if (np - opts.num_edge_entries > interval[1] && opts.ellipse_text) {
                    jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
                var begin = Math.max(np - opts.num_edge_entries, interval[1]);
                for (var i = begin; i < np; i++) {
                    appendItem(i);
                }

            }
            if (opts.next_text && (current_page < np - 1 || opts.next_show_always)) {
                appendItem(current_page + 1, { text: opts.next_text, classes: "next" });
            }
            // 末页
            if (opts.showend) {
                appendItem((np - 1), { text: '末页', classes: "" });
            }
            // 跳转
            if (opts.showjump) {
                var html = [];
                html.push('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;');
                html.push('第<input id="pagingNum" name="pagingNum" class="input-small" style="text-align:center;" type="text" value="' + (current_page >= 0 ? (current_page + 1) : '') + '" />页');
                html.push('&nbsp;&nbsp;&nbsp;&nbsp;');
                panel.append(html.join(''));
                panel.append($('<a class="button-white" style="cursor:pointer;"><span>跳转</span></a>').bind("click", GotoBtn));
            }
        }
        var current_page = opts.current_page;
        maxentries = (!maxentries || maxentries < 0) ? 1 : maxentries;
        opts.items_per_page = (!opts.items_per_page || opts.items_per_page < 0) ? 1 : opts.items_per_page;
        var panel = jQuery(this);
        this.selectPage = function (page_id) { pageSelected(page_id); }
        this.prevPage = function () {
            if (current_page > 0) {
                pageSelected(current_page - 1);
                return true;
            }
            else {
                return false;
            }
        }
        this.nextPage = function () {
            if (current_page < numPages() - 1) {
                pageSelected(current_page + 1);
                return true;
            }
            else {
                return false;
            }
        }

        drawLinks();
    });
}