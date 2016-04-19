var gulp = require('gulp'),
	sass = require('gulp-sass'),
	minifyCSS = require("gulp-minify-css"),
	runSequence = require('run-sequence'),
	clean = require('gulp-clean'),
	csslint = require('gulp-csslint'),
	watch = require("gulp-watch"),
		src = {
			styles: '../Styles/style.scss',
			stylesWatch: ['../Styles/style.scss', '../Styles/SASS/**'],
			lintcss : '../styles/style.css',
		},
		dest = {
			root: '../static/**',
			styles: '../static/styles/',
	};

gulp.task('styles', function () {
    return gulp.src(src.styles)
		.pipe(sass({ errLogToConsole: true }))
		.pipe(minifyCSS())
		.pipe(gulp.dest(dest.styles));
});

gulp.task('clean', function() {
	return gulp.src(dest.root, { read: false })
		.pipe(clean({ force: true }));
});

gulp.task('lintcss', function() {
	return gulp.src(src.lintcss)
		.pipe(csslint())
    	.pipe(csslint.reporter());
});

gulp.task('watch', function () {
	gulp.watch(src.stylesWatch, ['styles']);

});

// all is used to exclude watch task
gulp.task('all', function() {
	runSequence('clean', ['styles']);
});
gulp.task('default', ['all', 'watch']);