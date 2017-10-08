+(function ($) {
    "use strict";

    var pluginName = 'dialog';

    function Modal(options) {
        this.options = $.extend({}, $.fn[pluginName].defaults, options);
        this.$modal = $(this.options.target).attr('class', 'modal fade').hide();
        var self = this;

        function init() {
            if (self.options.title === '') {
                self.options.title = '&nbsp;';
            }
        };

        init();
    }


    $.extend(Modal.prototype, {
        show: function () {
            var self = this
				, $backdrop;

            if (!this.options.nobackdrop) {
                $backdrop = $('.modal-backdrop');
            }
            if (!this.$modal.length) {
                this.$modal = $('<div class="modal fade" id="' + this.options.target.substr(1) + '"><div class="modal-dialog"><div class="modal-content"/></div></div>').appendTo(this.options.appendTo).hide();
            }

            this.$modal.find('.modal-header h4').html(this.options.title);

            if (this.options.cssclass !== undefined) {
                this.$modal.attr('class', 'modal fade ' + this.options.cssclass);
            }

            if (this.options.width !== undefined) {
                this.$modal.width(this.options.width);
            }

            if (this.options.left !== undefined) {
                this.$modal.css({ 'left': this.options.left });
            }

            if (this.options.height !== undefined) {
                this.$modal.height(this.options.height);
            }

            if (this.options.top !== undefined) {
                this.$modal.css({ 'top': this.options.top });
            }

            if (this.options.keyboard) {
                this.escape();
            }

            if (!this.options.nobackdrop) {
                if (!$backdrop.length) {
                    $backdrop = $('<div class="modal-backdrop fade" />').appendTo(this.options.appendTo);
                }
                $backdrop[0].offsetWidth; // force reflow
                $backdrop.addClass('in');
            }

            this.$modal.off('close.' + pluginName).on('close.' + pluginName, function () {
                self.close.call(self);
            });
            if (this.options.remote !== undefined && this.options.remote != '' && this.options.remote !== '#') {
                var spinner;
                if (typeof Spinner == 'function') {
                    spinner = new Spinner({ color: '#3d9bce' }).spin(this.$modal[0]);
                }
                this.$modal.find('.modal-content').load(this.options.remote, function () {
                    if (spinner) {
                        spinner.stop();
                    }
                    if (self.options.cache) {
                        self.options.content = $(this).html();
                        delete self.options.remote;
                    }
                });
            } else {
                this.$modal.find('.modal-content').html(this.options.content);
            }

            this.$modal.show().addClass('in');
            return this;
        }

		, close: function () {
		    this.$modal.hide().off('.' + pluginName).find('.modal-content').html('');
		    if (this.options.cssclass !== undefined) {
		        this.$modal.removeClass(this.options.cssclass);
		    }
		    $(document).off('keyup.' + pluginName);
		    $('.modal-backdrop').remove();
		    if (typeof this.options.onClose === 'function') {
		        this.options.onClose.call(this, this.options);
		    }
		    return this;
		}

		, destroy: function () {
		    this.$modal.remove();
		    $(document).off('keyup.' + pluginName);
		    $('.modal-backdrop').remove();
		    this.$modal = null;
		    return this;
		}

		, escape: function () {
		    var self = this;
		    $(document).on('keyup.' + pluginName, function (e) {
		        if (e.which == 27) {
		            self.close();
		        }
		    });
		}
    });


    $.fn[pluginName] = function (options) {
        return this.each(function () {
            var obj;
            if (!(obj = $.data(this, pluginName))) {
                var $this = $(this)
					, data = $this.data()
					, opts = $.extend({}, options, data)
                ;
                if ($this.attr('href') !== '' && $this.attr('href') != '#') {
                    opts.remote = $this.attr('href');
                }
                obj = new Modal(opts);
                $.data(this, pluginName, obj);
            }
            obj.show();
        });
    };


    $[pluginName] = function (options) {
        return new Modal(options);
    };


    $.fn[pluginName].defaults = {
        title: '&nbsp;'		// modal title
		, target: '#modal'	// the modal id. MUST be an id for now.
		, content: ''		// the static modal content (in case it's not loaded via ajax)
		, appendTo: 'body'	// where should the modal be appended to (default to document.body). Added for unit tests, not really needed in real life.
		, cache: false		// should we cache the output of the ajax calls so that next time they're shown from cache?
		, keyboard: false
		, nobackdrop: false
    };


    $(document).on('click.' + pluginName, '[data-trigger="modal"]', function () {
        $(this)[pluginName]();
        if ($(this).is('a')) {
            return false;
        }
    }).on('click.' + pluginName, '[data-dismiss="modal"]', function (e) {
        e.preventDefault();
        $(this).closest('.modal').trigger('close');
    });
})(jQuery);