import { Button, Menu } from "@mantine/core";
import { IconBriefcase, IconEdit, IconRotate2 } from "@tabler/icons-react";
import { GameStatus } from "../../common/schemas";
import { useNavigate } from "react-router-dom";

interface ManageGameButtonProps {
  gameId: string;
  gameStatus: GameStatus;
  openResultGame(): void;
  width: "full" | "fit-content";
}

export default function ManageGameButton({
  gameId,
  gameStatus,
  openResultGame,
  width,
}: ManageGameButtonProps) {
  const navigate = useNavigate();
  return (
    <Menu withinPortal shadow="sm">
      <Menu.Target>
        <Button
          fullWidth={width === "full"}
          w={width === "fit-content" ? "fit-content" : undefined}
          color={"gray"}
          leftSection={<IconBriefcase size={18} />}
          className="manage-game-button"
        >
          Manage
        </Button>
      </Menu.Target>
      <Menu.Dropdown>
        <Menu.Item
          disabled={gameStatus === "resulted"}
          leftSection={<IconEdit size={18} />}
          onClick={() => navigate(`/games/${gameId}/edit`)}
        >
          Edit
        </Menu.Item>
        <Menu.Item
          disabled={gameStatus !== "closed"}
          leftSection={<IconRotate2 size={18} />}
          onClick={openResultGame}
        >
          Result
        </Menu.Item>
      </Menu.Dropdown>
    </Menu>
  );
}
