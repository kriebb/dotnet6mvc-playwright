const gulp = require('gulp');
const exec = require('child_process').exec;
const os = require('os');

gulp.task('copyJs', function () {
    return gulp.src([
        'node_modules/**/*.min.js',
        'node_modules/**/bootstrap.min.css',

        // Add other JS libraries paths as needed
    ])
        .pipe(gulp.dest('wwwroot/lib'));
});

gulp.task('clean-lib', function (cb) {
    console.log("Removing wwwroot/lib...");

    let command = '';

    if (os.platform() === 'win32') { // if Windows
        command = 'rmdir /s /q wwwroot\\lib';
    } else { // if Linux or macOS
        command = 'rm -rf wwwroot/lib';
    }

    exec(command, function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        if (err !== null) {
            console.error('Error deleting wwwroot/lib: ', err);
            cb(new Error('Error deleting wwwroot/lib'));
        } else {
            console.log("wwwroot/lib removed.");
            cb();
        }
    });
});

gulp.task('clean-node-modules', function (cb) {
    console.log("Removing node_modules...");

    let command = '';

    if (os.platform() === 'win32') { // if Windows
        command = 'rmdir /s /q node_modules';
    } else { // if Linux or macOS
        command = 'rm -rf node_modules';
    }

    exec(command, function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        if (err !== null) {
            console.error('Error deleting node_modules: ', err);
            cb(new Error('Error deleting node_modules'));
        } else {
            console.log("node_modules removed.");
            cb();
        }
    });
});

gulp.task('npm-install', function (cb) {
    console.log("Running npm install...");
    exec('npm install', function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        if (err !== null) {
            console.error('npm install error: ', err);
            cb(new Error('npm install failed'));
        } else {
            console.log("npm install finished.");
            cb();
        }
    });
});

gulp.task('default', gulp.series(
    'clean-node-modules',
    'clean-lib',

    'npm-install',
    'copyJs'
));
