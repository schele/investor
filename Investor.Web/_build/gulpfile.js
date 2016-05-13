var gulp = require('gulp'),
	sass = require('gulp-sass'),
	concat = require('gulp-concat'),
	minifyCSS = require("gulp-minify-css"),
	runSequence = require('run-sequence'),
	clean = require('gulp-clean'),
	watch = require("gulp-watch"),
		src = {
			resetStyles: '../SASS/reset.scss',
			styles: '../SASS/style.scss',
			stylesWatch: ['../SASS/style.scss', '../SASS/Styles/**'],
			js: '../Scripts/*.js'
		},
		dest = {
			root: '../static/**',
			resetStyles: '../static/styles/',
			styles: '../static/styles/',
			js: '../static/scripts/'
	};

gulp.task('styles', function () {
    return gulp.src(src.styles)
		.pipe(sass({ errLogToConsole: true }))
		.pipe(minifyCSS())
		.pipe(gulp.dest(dest.styles));
});

gulp.task('scripts', function () {
    return gulp.src(src.js)
		.pipe(concat('app.js'))
		.pipe(gulp.dest(dest.js));
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
	gulp.watch(src.js, ['scripts']);
});

// all is used to exclude watch task
gulp.task('all', function() {
	runSequence('clean', ['styles', 'resetStyles', 'scripts']);
});
gulp.task('default', ['all', 'watch']);