(function ($) {

    $.extend({
        filer: function (path, callback) {
            //var defaults = {};
            //var settings = $.extend({}, defaults, options);

            var dialog = $('<div class="modal-backdrop fade in"></div><div class="modal in" aria-hidden="false" tabindex="-1" style="display: block;"><div class="modal-dialog">  <div class="modal-content">    <div class="modal-header">      <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>      <h4 class="modal-title">选择文件</h4>    </div>    <div class="modal-body"></div><div class="modal-footer"><button href="#" class="btn btn-primary note-image-btn disabled" disabled="">Insert Image</button></div>  </div></div></div>');

            $.getJSON("/global/weed/", { dir: path }, function (r) {


                dialog.find('.modal-body').html(buildHtml(r));
                dialog.find('.close').click(function () {

                    $(dialog).remove();
                });
                $(document.body).append(dialog);

            });
            function onDirectoryClick(d) {
                // var d = data.files[i];
                console.log(d);
            }

            function onFileClick(e) {
                e.selected = !e.selected || false;                
                console.log(e.selected);
            }

            function buildHtml(data) {
                var df = document.createDocumentFragment();
                var ul = document.createElement("ul");
                if (data.directories) {
                    for (var i = 0; i < data.directories.length; i++) {
                        var o = data.directories[i];
                        var li = document.createElement("li");
                        var span = document.createElement("span");
                        span.innerText = o.name;
                        span.data = o;
                        span.onclick = function (e) {
                            onclick(e.target);
                        }
                        li.appendChild(span);
                        ul.appendChild(li);
                    }
                }
                if (data.files) {
                    for (var i = 0; i < data.files.length; i++) {
                        var o = data.files[i];
                        var li = document.createElement("li");
                        var span = document.createElement("span");
                        span.innerText = o.name;
                        span.data = o;
                        span.onclick = function (e) {
                            onFileClick(e.target);
                        }
                        li.appendChild(span);
                        ul.appendChild(li);
                    }
                }
                df.appendChild(ul);
                return df;
            }
        }
    });
}(jQuery));