import { useEffect, useMemo, useRef, useState } from 'react';
import BrandLogo from './BrandLogo';
import BrowserWindow from './BrowserWindow';
import type { TouristPoint, TouristPointPayload } from '../services/api';

type FormMode = 'create' | 'edit' | 'details';

interface Props {
  deleting?: boolean;
  error: string;
  onCancel: () => void;
  onDelete?: () => void;
  onEdit?: () => void;
  onGoHome: () => void;
  onSave?: (payload: TouristPointPayload) => Promise<void>;
  mode?: FormMode;
  point?: TouristPoint | null;
  saving?: boolean;
}

const brazilianStates = [
  'AC',
  'AL',
  'AP',
  'AM',
  'BA',
  'CE',
  'DF',
  'ES',
  'GO',
  'MA',
  'MT',
  'MS',
  'MG',
  'PA',
  'PB',
  'PR',
  'PE',
  'PI',
  'RJ',
  'RN',
  'RS',
  'RO',
  'RR',
  'SC',
  'SP',
  'SE',
  'TO',
];

function TouristPointForm({
  deleting = false,
  error,
  mode = 'create',
  onCancel,
  onDelete,
  onEdit,
  onGoHome,
  onSave,
  point,
  saving = false,
}: Props) {
  const nameInputRef = useRef<HTMLInputElement>(null);
  const buildFormState = (): TouristPointPayload => {
    if (!point) {
      return {
        city: '',
        description: '',
        location: '',
        name: '',
        state: '',
      };
    }

    return {
      city: point.city,
      description: point.description,
      location: point.location,
      name: point.name,
      state: point.state,
    };
  };

  const [formData, setFormData] = useState<TouristPointPayload>(() => buildFormState());

  const remainingCharacters = useMemo(
    () => Math.max(100 - formData.description.length, 0),
    [formData.description.length],
  );
  const isReadOnly = mode === 'details';
  const heading =
    mode === 'details'
      ? 'Detalhes do ponto turístico'
      : mode === 'edit'
        ? 'Editar ponto turístico'
        : 'Cadastrar ponto turístico';
  const eyebrow =
    mode === 'details'
      ? 'Visualizacao'
      : mode === 'edit'
        ? 'Atualizacao de cadastro'
        : 'Novo cadastro';

  useEffect(() => {
    if (mode === 'create') {
      nameInputRef.current?.focus();
    }
  }, [mode]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!onSave || isReadOnly) {
      return;
    }

    await onSave(formData);
  };

  return (
    <BrowserWindow className="browser-window--form">
      <div className="window-toolbar">
        <BrandLogo alt="Logotipo da tela de cadastro" onClick={onGoHome} />

        <div className="window-intro">
          <p className="window-intro__eyebrow">{eyebrow}</p>
          <h2>{heading}</h2>
        </div>
      </div>

      {error && <div className="feedback feedback--error">{error}</div>}

      <form className="editor-form" onSubmit={handleSubmit}>
        <section className="editor-section">
          <span className="editor-section__title">Nome</span>
          <input
            aria-label="Nome"
            className="control"
            maxLength={100}
            onChange={(event) =>
              setFormData((current) => ({ ...current, name: event.target.value }))
            }
            placeholder="Nome do ponto turístico"
            readOnly={isReadOnly}
            ref={nameInputRef}
            required
            type="text"
            value={formData.name}
          />
        </section>

        <section className="editor-section">
          <span className="editor-section__title">Localização</span>
          <div className="editor-location-grid">
            <select
              aria-label="UF"
              className="control"
              disabled={isReadOnly}
              onChange={(event) =>
                setFormData((current) => ({ ...current, state: event.target.value }))
              }
              required
              value={formData.state}
            >
              <option value="">UF</option>
              {brazilianStates.map((state) => (
                <option key={state} value={state}>
                  {state}
                </option>
              ))}
            </select>

            <input
              aria-label="Cidade"
              className="control"
              readOnly={isReadOnly}
              onChange={(event) =>
                setFormData((current) => ({ ...current, city: event.target.value }))
              }
              placeholder="Cidade"
              required
              type="text"
              value={formData.city}
            />
          </div>
          <input
            aria-label="Referência"
            className="control"
            readOnly={isReadOnly}
            onChange={(event) =>
              setFormData((current) => ({ ...current, location: event.target.value }))
            }
            placeholder="Referência"
            required
            type="text"
            value={formData.location}
          />
        </section>

        <section className="editor-section">
          <span className="editor-section__title">Descritivo</span>
          <div className="editor-section__stack">
            <textarea
              aria-label="Descritivo"
              className="control control--textarea"
              maxLength={100}
              readOnly={isReadOnly}
              onChange={(event) =>
                setFormData((current) => ({
                  ...current,
                  description: event.target.value,
                }))
              }
              placeholder="Resumo com no maximo 100 caracteres"
              required
              rows={5}
              value={formData.description}
            />
            <span className="editor-row__hint">
              {remainingCharacters} caracteres restantes
            </span>
          </div>
        </section>

        <div className="editor-actions-row">
          <div className="editor-actions">
            {isReadOnly ? (
              <>
                <button className="button button--ghost" onClick={onCancel} type="button">
                  voltar
                </button>
                <div className="editor-actions__group">
                  <button className="button" onClick={onEdit} type="button">
                    editar
                  </button>
                  <button
                    className="button button--danger"
                    disabled={deleting}
                    onClick={onDelete}
                    type="button"
                  >
                    {deleting ? 'excluindo...' : 'excluir'}
                  </button>
                </div>
              </>
            ) : (
              <>
                <div className="editor-actions__group">
                  <button className="button button--primary" disabled={saving} type="submit">
                    {saving ? 'salvando...' : point ? 'atualizar' : 'cadastrar'}
                  </button>
                </div>
                <button
                  className="button button--ghost editor-actions__back"
                  onClick={onCancel}
                  type="button"
                >
                  voltar
                </button>
              </>
            )}
          </div>
        </div>
      </form>
    </BrowserWindow>
  );
}

export default TouristPointForm;
