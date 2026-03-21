import type { ReactNode } from 'react';

interface Props {
  children: ReactNode;
  className?: string;
}

function BrowserWindow({ children, className = '' }: Props) {
  return (
    <section className={`browser-window ${className}`.trim()}>
      <div className="browser-window__content">{children}</div>
    </section>
  );
}

export default BrowserWindow;
