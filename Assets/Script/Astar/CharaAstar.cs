using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class CharaAstar : MonoBehaviour {
    private List<Vector3Int> Tile_Pos;
    private List<List<bool>> isBattle;
    private Board board;
    [SerializeField] int Lerp_x = 5;
    [SerializeField] int Lerp_y = 6;
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    private bool ismove = false;
    private GameObject Target_Object;  
    private string targetTag;

    Vector2Int Coord_Lerp() {
        Vector3Int pos = this.GetComponent<CharaLocate>().Player_Tilepos();
        int Lerp_pos_x = pos.x + Lerp_x;
        int Lerp_pos_y = pos.y + Lerp_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_pos_x, Lerp_pos_y);
        return Lerp_pos;
    }

    void Start() {
        board = new Board(width, height);
        Blocks_set();
        if (this.transform.tag == "Friendly") targetTag = "Enemy";
        else if (this.transform.tag == "Enemy") targetTag = "Friendly";
    }

    void Blocks_set() {
        board.blocks = new Block[width, height];
        for (int i = 0; i < board.blocks.GetLength(0); i++) {
            for (int j = 0; j < board.blocks.GetLength(1); j++) {
                board.blocks[i, j] = new Block(i, j);
            }
        }
    }

    Vector2Int Target_Coord_Lerp() {
        Vector3Int pos = GameManager.instance.tilemap.LocalToCell(Target_Object.transform.position);
        int Lerp_pos_x = pos.x + Lerp_x;
        int Lerp_pos_y = pos.y + Lerp_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_pos_x, Lerp_pos_y);
        return Lerp_pos;
    }

    bool Exists(int x, int y) {
        if(x >= 0 && y >= 0 && isBattle[x][y]) return true;
        else return false;
    }

    void TileCheck() {
        //isBattle = GameManager.instance.isBattle;
        Vector2Int target = Coord_Lerp();
        List<Block> arounds = new List<Block>();
        if (Exists(target.x - 1, target.y - 1)) {
            Block block = board.blocks[target.x - 1, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y - 1))
        {
            Block block = board.blocks[target.x, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y - 1))
        {
            Block block = board.blocks[target.x + 1, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x - 1, target.y))
        {
            Block block = board.blocks[target.x - 1, target.y];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y))
        {
            Block block = board.blocks[target.x + 1, target.y];
            arounds.Add(block);
        }

        if (Exists(target.x - 1, target.y + 1))
        {
            Block block = board.blocks[target.x - 1, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y + 1))
        {
            Block block = board.blocks[target.x, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y + 1))
        {
            Block block = board.blocks[target.x + 1, target.y + 1];
            arounds.Add(block);
        }
    }
    
    public Block CreatePath() {
        pos = this.GetComponent<CharaLocate>().Player_Tilepos();
        Vector2Int start_pos = Coord_Lerp();
        Vector2Int dest_pos = Target_Coord_Lerp();
        TileCheck();
        Block start = board.blocks[start_pos.x, start_pos.y];
        Block dest = board.blocks[dest_pos.x, dest_pos.y];
        var startBlock = PathFindTile(start, dest);
        if(startBlock != null) return startBlock;
        return null;
    }

    Vector3Int pos;
    Block startBlock = null;
    Coroutine Movetile;
    public bool isCoroutin = true;

    public void MovePath() {
        if(Target_Object == null) Target_Object = this.GetComponent<CharaController>().Set_Target(targetTag);
        
        if(Target_Object == null) return;
        else if(startBlock == null) startBlock = CreatePath();

        if(startBlock != null) {
            if(!ismove) Movetile = StartCoroutine(MoveTile(startBlock));
            else {
                ismove = false;
                startBlock = startBlock.next;
                MovePath();
            }
        }
        else return;
    }

    public void StopPath() {
        if(Movetile == null) return;
        else StopCoroutine(Movetile);
    }

    IEnumerator MoveTile(Block target_Block) {
        if(!isCoroutin) yield break;
        ismove = true;
        Vector3Int player_pos;
        if(target_Block.prev != null) player_pos = new Vector3Int(target_Block.prev.x - Lerp_x,target_Block.prev.y - Lerp_y, pos.z);
        else player_pos = new Vector3Int(pos.x, pos.y, pos.z);    
        Vector3Int target_pos = new Vector3Int(target_Block.x - Lerp_x, target_Block.y - Lerp_y, pos.z);
        Vector3 pos_Lerp = GameManager.instance.tilemap.CellToWorld(player_pos);
        Vector3 target_Lerp = GameManager.instance.tilemap.CellToWorld(target_pos);

        int loopNum = 0;
        float elapsedTime = 0f;
        float speedValue = Vector3.Magnitude(target_Lerp - pos_Lerp);
        
        while(Vector3.Magnitude(target_Lerp - transform.position) > 0.01f) { 
            transform.position = Vector3.Lerp(pos_Lerp, target_Lerp, elapsedTime += Time.deltaTime/speedValue);
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(target_Lerp - pos_Lerp), elapsedTime);
            if (loopNum++ > 10000) throw new Exception("Infinite Loop");   
            yield return null;  
        }
        transform.position = target_Lerp;
        transform.rotation =  Quaternion.LookRotation(target_Lerp - pos_Lerp);
        startBlock = CreatePath();
        MovePath();
    }
   
    Block PathFindTile(Block start, Block dest) {   
        if(board.Exists(start) && board.Exists(dest)) {
            board.CheckClear();
            List<Block> waittingBlocks = new List<Block>();
            List<Block> finishedBlocks = new List<Block>();
            Block currentBlock = start;

            while(currentBlock != null) {
                var aroundBlocks = board.GetAroundBlocks(currentBlock);

                for(int i = 0; i < aroundBlocks.Count; i++) {
                    var block = aroundBlocks[i];
                    if(!waittingBlocks.Equals(block) && !block.check) {
                        waittingBlocks.Add(block);
                    }
                }

                currentBlock.check = true;
                if(waittingBlocks.Remove(currentBlock)) finishedBlocks.Add(currentBlock);

                if(aroundBlocks.Count == 0) return null;
                else {
                    aroundBlocks = aroundBlocks.FindAll(block => !block.check);
                }

                CalcRating(aroundBlocks, start, currentBlock, dest);

                currentBlock = GetNextBlock(aroundBlocks, currentBlock);
                if(currentBlock == null) {
                    currentBlock = GetPriorityBlock(waittingBlocks);

                    if(currentBlock == null) {
                        Block exceptionBlock = null;
                        for(int i = 0; i < finishedBlocks.Count; i++) {
                            if(exceptionBlock == null || exceptionBlock.H > finishedBlocks[i].H) exceptionBlock = finishedBlocks[i];

                        }
                        currentBlock = exceptionBlock;
                        break;
                    }
                }
                else if(currentBlock.Equals(dest)) break;
            }
            while(!currentBlock.Equals(start)) {
                currentBlock.prev.next = currentBlock;
                currentBlock = currentBlock.prev; 
            }

            start.prev = null;
            return start;
        }
        return null;
    }

    Block GetPriorityBlock(List<Block> waittingBlocks) {
        Block block = null;
        var enumerator = waittingBlocks.GetEnumerator();
        while(enumerator.MoveNext()) {
            var current = enumerator.Current;
            if(block == null || block.F < current.F) block = current;
        }
        return block;
    }

    Block GetNextBlock(List<Block> arounds, Block current) {
        Block minValueBlock = null;
        
        for(int i = 0; i < arounds.Count; i++) {
            Block next = arounds[i];
            if(!next.check) {
                if(minValueBlock == null) minValueBlock = next;
                else if(minValueBlock.H > next.H) minValueBlock = next;
                else continue;
            }
        }
    
        return minValueBlock;
    }

    void CalcRating(List<Block> arounds, Block start, Block current, Block dest) {
        if(arounds !=null) {
            for(int i = 0; i < arounds.Count; i++) {
                var block = arounds[i];
                bool isDiagonalBlock = Math.Abs(block.x - current.x) == 1 && Math.Abs(block.y - current.y) == 1;
                int priceFromDest = (Math.Abs(dest.x - block.x) + Math.Abs(dest.y - block.y)) * 10;
                if (block.prev == null) block.prev = current;
                block.SetPrice(current.G + (isDiagonalBlock ? 14 : 10), priceFromDest);
            }
        }
    }
}
