export default function Loader() {
    const circleCommonClasses = 'h-2.5 w-2.5 bg-black rounded-full';

    return (
        <div className="flex">
            <div className={`${circleCommonClasses} mr-1 animate-bounce`}></div>
            <div className={`${circleCommonClasses} mr-1 animate-bounce-200`}></div>
            <div className={`${circleCommonClasses} animate-bounce-400`}></div>
        </div>
    );
}