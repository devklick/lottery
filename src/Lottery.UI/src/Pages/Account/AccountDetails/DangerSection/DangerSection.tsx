import { Button, Collapse, Divider, Group } from "@mantine/core";
import { useDisclosure, useHover } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import { useRef } from "react";

import ConfirmDeleteAccountModal from "./ConfirmDeleteAccountModal";
import DeleteAccountButton, {
  DeleteAccountButtonRef,
} from "./DeleteAccountButton";
import { useColorShadeForTheme } from "../../../../common/hooks/theme.hooks";

export default function DangerSection() {
  const [expanded, { toggle: toggleExpanded }] = useDisclosure();
  const [modalOpened, { close: closeModal, open: openModal }] = useDisclosure();
  const { ref, hovered } = useHover<HTMLButtonElement>();
  const buttonRef = useRef<DeleteAccountButtonRef>(null);

  function onCloseModal() {
    buttonRef.current?.reset();
    closeModal();
  }

  const showRed = hovered || expanded;
  const baseColor = useColorShadeForTheme({ color: "dark", dark: 4, light: 3 });

  return (
    <>
      <Divider
        w="100%"
        color={showRed ? "red" : baseColor}
        labelPosition="left"
        label={
          <Group onClick={toggleExpanded}>
            <Button
              ref={ref}
              size="compact-xs"
              color={showRed ? "red" : baseColor}
              rightSection={
                expanded ? (
                  <IconChevronUp size={12} />
                ) : (
                  <IconChevronDown size={12} />
                )
              }
            >
              Danger
            </Button>
          </Group>
        }
      />
      <Collapse expanded={expanded} w="100%">
        <Group justify="end">
          <DeleteAccountButton ref={buttonRef} onClicked={openModal} />
        </Group>
      </Collapse>

      <ConfirmDeleteAccountModal
        opened={modalOpened}
        closeModal={onCloseModal}
      />
    </>
  );
}
