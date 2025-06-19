window.sessionStudentService = {
    save: function (studentsJson) {
        sessionStorage.setItem('students', studentsJson);
    },
    load: function () {
        return sessionStorage.getItem('students');
    }
};
