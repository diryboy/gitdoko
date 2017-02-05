var gulp = require('gulp');

gulp.task('build_3rd_party', () => {
    var modify = require('gulp-modify-file');
    var less = require('gulp-less');
    var cleanCss = require('gulp-clean-css');
    var rename = require('gulp-rename');
    var Path = require('path');

    gulp.src([
        'node_modules/*/**/bootstrap.less',
        'node_modules/*/**/AdminLTE.less',
        'node_modules/*/**/_all-skins.less'
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
        paths: [Path.join(__dirname, 'src/3rd')]
    }))
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/css';
    }))
    .pipe(gulp.dest('3rd_party'))
    .pipe(cleanCss())
    .pipe(rename({
        suffix: ".min"
    }))
    .pipe(gulp.dest('3rd_party'));

    gulp.src([
        'node_modules/*/fonts/*.*'
    ])
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/fonts';
    }))
    .pipe(gulp.dest('3rd_party'));

    gulp.src([
        'node_modules/*/dist/*.js',
        'node_modules/*/dist/js/*.js',
        'node_modules/jquery-validation-unobtrusive/*.js',
    ])
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/js';
    }))
    .pipe(gulp.dest('3rd_party'));

    function getNodeModuleName(path) {
        return path.match(/\.|[\w\-]+/)[0];
    }
});
