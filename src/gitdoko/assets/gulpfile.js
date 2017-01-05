var gulp = require('gulp');
var modify = require('gulp-modify-file');

gulp.task('build_3rd_party', ['transform_3rd_party'], function () {
});

gulp.task('transform_3rd_party', function () {
    var originalSrcFiles = [
        'bootstrap/less/bootstrap.less',
        'admin-lte/build/less/AdminLTE.less'
    ];
    for (var i = 0; i < originalSrcFiles.length; i++) {
        let libFile = originalSrcFiles[i];
        gulp.src('node_modules/' + libFile)
            .pipe(modify(function (content, path, file) {

                var libName = libFile.match(/(.*?)\//)[1];
                var varName = libName + '-dir';

                content = '@' + varName + ': "../../node_modules/' + libFile.match(/.*\//)[0] + '";\n' + content;
                content = content.replace(/(@import url\()/g, '// $1');
                content = content.replace(/(@import ")/g, '$1@{' + varName + '}');

                var newBootstrapVars = '../bootstrap-variables.less'
                content = content.replace('@{bootstrap-dir}variables.less', newBootstrapVars);
                content = content.replace('../bootstrap-less/variables.less', newBootstrapVars);

                content = content.replace('../bootstrap-less', '../../node_modules/bootstrap/less');

                return content;
            }))
            .pipe(gulp.dest('3rd_party/src'));
    }
});