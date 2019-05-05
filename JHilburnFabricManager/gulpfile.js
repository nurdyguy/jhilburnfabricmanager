"use strict";

var gulp = require("gulp"),
	rimraf = require("rimraf"),
	concat = require("gulp-concat"),
	cssmin = require("gulp-cssmin"),
	sass = require("gulp-sass"),
	sourcemaps = require("gulp-sourcemaps"),
	rename = require("gulp-rename"),
	merge = require("merge-stream"),
	uglify = require("gulp-uglify"),
	bundleconfig = require("./bundleconfig.json");

var regex = {
	css: /\.css$/,
	html: /\.(html|htm)$/,
	js: /\.js$/,
	scss: /\.scss$/
};

gulp.task("build", ["sass", "min:js"]);

gulp.task("sass", ["sass:Compile", "css:Bundle", "css:Minify"]);

gulp.task("sass:Compile", function () {
	var tasks = BuildSassFileTasks('wwwroot/scss/*.scss');
	return merge(tasks);
});

gulp.task("css:Bundle", ["sass:Compile"], function () {
	var tasks = getBundles(regex.css).map(function (bundle) {
		return gulp.src(bundle.inputFiles, { base: "." })
			.pipe(concat(bundle.outputFileName))
			.pipe(gulp.dest("."));
	});
	return merge(tasks);
});

gulp.task("css:Minify", ["css:Bundle"], function () {
	var tasks = getBundles(regex.css).map(function (bundle) {
		return gulp.src(bundle.inputFiles, { base: "." })
			.pipe(concat(bundle.outputFileName))
			.pipe(cssmin())
			.pipe(gulp.dest("."));
	});
	return merge(tasks);
});

gulp.task("clean", function () {
	var files = [];

	files.push('./wwwroot/css/**.css');
	files.push('./wwwroot/css/**.css.*');
	files.push('./wwwroot/scss/**.css');
	files.push('./wwwroot/scss/**.css.*');

	return del(files);
});

gulp.task("watch", function () {
	getBundles(regex.js).forEach(function (bundle) {
		gulp.watch(bundle.inputFiles, ["min:js"]);
	});

	gulp.watch('./wwwroot/scss/*.scss', ['sass']);
});


function BuildSassFileTasks(fileName) {
	var tasks = gulp.src(fileName)
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest('./wwwroot/scss'))
		.pipe(sourcemaps.init({ loadMaps: true }))
		.pipe(cssmin())
		.pipe(rename({ extname: '.min.css' }))
		.pipe(sourcemaps.write('./'))
		.pipe(gulp.dest('./wwwroot/scss'));
	return tasks;
}

function getBundles(regexPattern) {
	return bundleconfig.filter(function (bundle) {
		return regexPattern.test(bundle.outputFileName);
	});
}