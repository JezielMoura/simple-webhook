var containerStarted = false;

const resources = {
    success: {
        icon: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: #28a745"><path d="M12 2C6.486 2 2 6.486 2 12s4.486 10 10 10 10-4.486 10-10S17.514 2 12 2zm0 18c-4.411 0-8-3.589-8-8s3.589-8 8-8 8 3.589 8 8-3.589 8-8 8z"></path><path d="M9.999 13.587 7.7 11.292l-1.412 1.416 3.713 3.705 6.706-6.706-1.414-1.414z"></path></svg>`,
        color: '#28a745'
    },
    info: {
        icon: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: #17a2b8"><path d="M12 2C6.486 2 2 6.486 2 12s4.486 10 10 10 10-4.486 10-10S17.514 2 12 2zm0 18c-4.411 0-8-3.589-8-8s3.589-8 8-8 8 3.589 8 8-3.589 8-8 8z"></path><path d="M11 11h2v6h-2zm0-4h2v2h-2z"></path></svg>`,
        color: '#17a2b8'
    },
    warning: {
        icon: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: #ffc107"><path d="M11.001 10h2v5h-2zM11 16h2v2h-2z"></path><path d="M13.768 4.2C13.42 3.545 12.742 3.138 12 3.138s-1.42.407-1.768 1.063L2.894 18.064a1.986 1.986 0 0 0 .054 1.968A1.984 1.984 0 0 0 4.661 21h14.678c.708 0 1.349-.362 1.714-.968a1.989 1.989 0 0 0 .054-1.968L13.768 4.2zM4.661 19 12 5.137 19.344 19H4.661z"></path></svg>`,
        color: '#ffc107'
    },
    error: {
        icon: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: #dc3545"><path d="M11.953 2C6.465 2 2 6.486 2 12s4.486 10 10 10 10-4.486 10-10S17.493 2 11.953 2zM12 20c-4.411 0-8-3.589-8-8s3.567-8 7.953-8C16.391 4 20 7.589 20 12s-3.589 8-8 8z"></path><path d="M11 7h2v7h-2zm0 8h2v2h-2z"></path></svg>`,
        color: '#dc3545'
    },
    closeIcon: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 384 512" style="fill: #aaaaaa">><path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z"/></svg>'
}

const styleContent = `
    .toast-container {
        position: fixed; top: 10px; right: 0px; left: 0; margin: auto; z-index: 9999; border-radius: 8px; width: 300px }

        .toast-container .toast {
            display: grid; grid-template: 1fr 4px / 44px 1fr 30px; margin-top: 10px; border-radius: 8px; overflow: hidden;
            box-shadow: 0 2px 10px var(--border); animation: fadeInOut 300ms ease-in-out; background: var(--color-bg); color: var(--color-fg) }

            .toast-container .toast img {
                grid-column: 1 / 2; grid-row: 1 / 3; align-self: center; justify-self: center }
            
            .toast-container .toast .content {
                grid-column: 2 / 4; grid-row: 1 / 3; font-size: 14px; margin-right: 10px; padding: 15px 0 }
                
            .toast-container .toast .content .title {
                margin: 0 0 7px 0; font-weight: 600 }
                    
            .toast-container .toast .content .message {
                margin: 0 }

            .toast-container .toast .close {
                grid-column: 3 / 4; grid-row: 1 / 2; align-self: start; padding: 7px; cursor: pointer }

            .toast-container .toast .progress-bar {
                grid-column: 1 / 4; grid-row: 2 / 3 }

    @keyframes fadeInOut {
        0% { opacity: 0; transform: translateY(-20px); }
        100% { opacity: 1; transform: translateY(0); }
    }

    @keyframes progress {
        0% { width: 100%; }
        100% { width: 0; }
    }
`;
  
type ToastOptions = {
    title?: string,
    duration?: number,
    type?: "success" | "info" | "warning" | "error" 
}

const startContainer = () => {
    const container = document.createElement('div');
    container.className = 'toast-container';
    const style = document.createElement('style');
    style.innerHTML = styleContent;
    document.head.append(style);
    document.body.append(container);
    return container;
}

const toast = (message: string, options: ToastOptions) => {
    const container = containerStarted ? document.querySelector('.toast-container') as HTMLDivElement : startContainer();
    containerStarted = true;
    const duration = options.duration || 5000;
    const resource = resources[options.type || 'info'];
    const toast = document.createElement('div');

    toast.className = 'toast';
    toast.innerHTML = `
        <img src=${'data:image/svg+xml;base64,' + btoa(resource.icon)} />
        <div class='content'>
            ${ options.title != undefined ? '<h4 class="title">' + options.title + '</h4>' : '' }
            <p class='message'>${message}</p>
        </div>
        <img class='close' src=${'data:image/svg+xml;base64,' + btoa(resources.closeIcon)} />
        <div class='progress-bar' style='background: ${resource.color}; animation: progress ${duration}ms linear'></div>
    `;

    // @ts-ignore
    toast.addEventListener('click', (e) => { if (e.target.matches("img")) toast.remove() });
    container?.appendChild(toast);
    setTimeout(() => toast.remove(), duration);
}

export { toast, type ToastOptions }
