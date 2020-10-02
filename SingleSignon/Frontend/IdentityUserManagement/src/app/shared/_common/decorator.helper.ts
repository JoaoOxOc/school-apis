import "reflect-metadata";
var __global__ClassName__id__ = 0;
export function ClassName(useThisName: string = null) {
    return function (target: any) {
       if (target.name.length === 0 || useThisName !== null) {
            Object.defineProperty(target, "name", {
                value: useThisName !== null ? useThisName : `__ClassName__${++__global__ClassName__id__}`,
                enumerable: false,
                writable: false,
                configurable: true
            });
        }
    }
}