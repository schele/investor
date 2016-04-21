var gulp = require('gulp'),
	sass = require('gulp-sass'),
	minifyCSS = require("gulp-minify-css"),
	runSequence = require('run-sequence'),
	clean = require('gulp-clean'),
	watch = require("gulp-watch"),
		src = {
			resetStyles: '../SASS/reset.scss',
			styles: '../SASS/style.scss',
			stylesWatch: ['../SASS/style.scss', '../SASS/Styles/**'],
		},
		dest = {
			root: '../static/**',
			resetStyles: '../static/styles/',
			styles: '../static/styles/',
	};

gulp.task('styles', function () {
    return gulp.src(src.styles)
		.pipe(sass({ errLogToConsole: true }))
		.pipe(minifyCSS())
		.pipe(gulp.dest(dest.styles));
});

gulp.task('resetStyles', function () {
    return gulp.src(src.resetStyles)
		.pipe(sass({ errLogToConsole: true }))
		.pipe(minifyCSS())
		.pipe(gulp.dest(dest.resetStyles));
});

gulp.task('clean', function() {
	return gulp.src(dest.root, { read: false })
		.pipe(clean({ force: true }));
});

gulp.task('watch', function () {
	gulp.watch(src.stylesWatch, ['styles']);

});

// all is used to exclude watch task
gulp.task('all', function() {
	runSequence('clean', ['styles', 'resetStyles']);
});
gulp.task('default', ['all', 'watch']);