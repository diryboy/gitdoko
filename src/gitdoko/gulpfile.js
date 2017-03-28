var gulp = require('gulp');
const static3rdPartyAssetsDir = 'assets/3rd_party';

gulp.task('build_3rd_party', () => {
    var modify = require('gulp-modify-file');
    var less = require('gulp-less');
    var cleanCss = require('gulp-clean-css');
    var rename = require('gulp-rename');
    var filter = require('gulp-filter');
    var Path = require('path');

    var fs = require('fs');
    var dependencies = JSON.parse(fs.readFileSync('package.json')).dependencies;

    gulp.src([
        'node_modules/*/**/bootstrap.less',
        'node_modules/*/**/AdminLTE.less',
        'node_modules/*/**/_all-skins.less'
    ])
    .pipe(modify((content, path, file) => {
        var newBootstrapVars = 'bootstrap-variables.less';
        if (path.indexOf('AdminLTE') > 0) {
            content = content.replace('@import url(', '// @import url(')
                             .replace('../bootstrap-less/variables.less', newBootstrapVars);
        } else {
            content = content.replace('variables.less', newBootstrapVars);
        }
        return content;
    }))
    .pipe(less({
        paths: [Path.join(__dirname, 'assets/src/3rd')]
    }))
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/css';
    }))
    .pipe(toStatic3rdPartyAssetsDir())
    .pipe(cleanCss())
    .pipe(rename({
        suffix: ".min"
    }))
    .pipe(toStatic3rdPartyAssetsDir());

    gulp.src([
        'node_modules/*/fonts/*.*'
    ])
    .pipe(filterToDependencies())
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/fonts';
    }))
    .pipe(toStatic3rdPartyAssetsDir());

    gulp.src([
        'node_modules/*/dist/*.js',
        'node_modules/*/dist/js/*.js',
        'node_modules/jquery-validation-unobtrusive/*.js'
    ])
    .pipe(filterToDependencies())
    .pipe(rename(path => {
        path.dirname = getNodeModuleName(path.dirname) + '/js';
    }))
    .pipe(toStatic3rdPartyAssetsDir());

    function getNodeModuleName(dirname) {
        return dirname.match(/\.|[\w\-]+/)[0];
    }

    function filterToDependencies() {
        return filter(file => {
            return file.path.match(/node_modules.([\w\-]+)/i)[1] in dependencies;
        });
    }

    function toStatic3rdPartyAssetsDir() {
        return gulp.dest(static3rdPartyAssetsDir);
    }
});
