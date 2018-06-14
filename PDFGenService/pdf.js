module.exports = function (callback, pdfHtml) {
    var jsreport = require('jsreport-core')();

    jsreport.init().then(function () {
        return jsreport.render({
            template: {
                content: pdfHtml,
                engine: 'jsrender',
                recipe: 'phantom-pdf'
            }
        }).then(function (res) {
            callback(/* error */ null, res.content.toJSON().data);
        });
    }).catch(function (e) {
        callback(/* error */ e, null);
    })
};