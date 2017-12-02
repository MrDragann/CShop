/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'ru';
    // config.uiColor = '#AADC6E';
    config.toolbar = [
	//{ name: 'document', items: ['Source', '-', 'Save', 'NewPage', 'DocProps', 'Preview', 'Print', '-', 'Templates'] },
	{ name: 'clipboard', items: ['Undo', 'Redo', '-', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'] },
    { name: 'insert', items: ['Embed', 'Image', 'VideoEmbed', 'Youtube', '-', 'Table', 'Smiley', 'SpecialChar'] },
	//{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
	//{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
'/',
	{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'RemoveFormat'] },
	{ name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
	{ name: 'links', items: ['Link', 'Unlink'] },
'/',
	{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
	{ name: 'colors', items: ['TextColor', 'BGColor'] },
	{ name: 'tools', items: ['Maximize'] }
    ];
    CKEDITOR.config.embed_provider = '//iframe.ly/api/oembed?url={url}&callback={callback}&api_key=c1925591690291d44d96c4';
    config.resize_enabled = false;
    config.extraPlugins = 'videoembed,youtube,autogrow,embed';
    config.autoGrow_onStartup = true;

    config.font_names =
    'Arial/Arial, Helvetica, sans-serif;' +
    'Comfortaa/Comfortaa,sans-serif;' +
    'Comic Sans MS/comic sans ms,sans-serif;' +
    'Courier New/courier new,courier,monospace;' +
    'Georgia/georgia,palatino,serif;' +
    'Helvetica/helvetica,arial,sans-serif;' +
    'Lobster/Lobster,Lobster-Regular;'
    'Tahoma/tahoma,arial,helvetica,sans-serif;' +
    'Times New Roman/times new roman,times,serif;' +
    'Verdana/verdana,geneva,sans-serif;';


    config.removePlugins = 'iframe';
    config.filebrowserUploadUrl = "http://" + window.location.hostname + ':' + window.location.port + "/Admin/Home/UploadImage";
};
