import { useState } from 'react';

interface Props {
  alt?: string;
  onClick?: () => void;
  src?: string;
}

const defaultLogoSrc = '/branding/logo.svg';

function BrandLogo({ alt = 'Logotipo da marca', onClick, src = defaultLogoSrc }: Props) {
  const [hasError, setHasError] = useState(false);

  const content = hasError ? (
    <>
      <span className="brand-stamp__title">Sua Marca</span>
      <span className="brand-stamp__subtitle">logo</span>
    </>
  ) : (
    <img
      alt={alt}
      className="brand-stamp__image"
      loading="eager"
      onError={() => setHasError(true)}
      src={src}
    />
  );

  if (onClick) {
    return (
      <button
        aria-label="Ir para a página inicial"
        className="brand-stamp brand-stamp--button"
        onClick={onClick}
        type="button"
      >
        {content}
      </button>
    );
  }

  return (
    <div className="brand-stamp" aria-label="Espaço reservado para a marca">
      {content}
    </div>
  );
}

export default BrandLogo;
