#include "pch.h"
#include <time.h>
#include <mkl.h>

extern "C"  _declspec(dllexport)
bool GetMKL(const int nx, float* x, const int ny, float* y, float *derivative, int& error, float *coef)
{
	DFTaskPtr Task;
	int code = dfsNewTask1D(&Task, nx, x, DF_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
	if (code != DF_STATUS_OK) {
		error = code;
		return false;
	}
	//float* coeff = new float[ny * 4 * (nx - 1)];
	code = dfsEditPPSpline1D(Task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END, NULL, DF_NO_IC, NULL, coef, DF_NO_HINT);  //свободные концы
	if (code != DF_STATUS_OK) {
		error = code;
		return false;
	}
	code = dfsConstruct1D(Task, DF_PP_SPLINE, DF_METHOD_STD);  //здесь метод - DF_METHOD_STD, указанный DF_METHOD_PP не работает (ошибка -1002)
	if (code != DF_STATUS_OK) {
		error = code;
		return false;
	}
	code = dfsInterpolate1D(Task, DF_INTERP, DF_METHOD_PP, nx, x,       //здесь же именно DF_METHOD_PP, DF_METHOD_STD не работает
		DF_UNIFORM_PARTITION, 2, new int[2]{ 0, 1 }, NULL, derivative,
		DF_MATRIX_STORAGE_ROWS, NULL);

	//code = dfsInterpolate1D(Task, DF_INTERP, DF_METHOD_PP, nx, x,   //чтобы проверить, что значения самой функции (0-й производной) правильные
	//	DF_UNIFORM_PARTITION, 2, new int[2]{ 1, 0 }, NULL, derivative,
	//	DF_MATRIX_STORAGE_ROWS, NULL);

	if (code != DF_STATUS_OK) {
		error = code;
		return false;
	}
	return true;
}