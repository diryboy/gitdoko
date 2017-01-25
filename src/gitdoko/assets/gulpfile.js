var gulp = require('gulp');

gulp.task('build_3rd_party', () => {
    var modify = require('gulp-modify-file');
    var less = require('gulp-less');
    var Path = require('path');

    gulp.src([
        'node_modules/bootstrap/less/bootstrap.less',
        'node_modules/admin-lte/build/less/AdminLTE.less'
    ])
    .pipe(modify((content, path, file) => {
        var newBootstrapVars = 'bootstrap-variables.less';
        if (path.indexOf('AdminLTE') > 0) {
            content = content.replace('@import url(', '// @import url(')
                             .replace('../bootstrap-less/variables.less', newBootstrapVars)
        } else {
            content = content.replace('variables.less', newBootstrapVars);
        }
        return content;
    }))
    .pipe(less({
        paths: [Path.join(__dirname, '3rd_party/src')]
    }))
    .pipe(gulp.dest('3rd_party'));
});
